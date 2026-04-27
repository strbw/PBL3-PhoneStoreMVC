using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HDKmall.BLL.Services
{
    public class GeminiChatService : IGeminiChatService
    {
        private readonly HttpClient _httpClient;
        private readonly IProductService _productService;
        private readonly IConfiguration _config;
        private readonly ILogger<GeminiChatService> _logger;
        private readonly IMemoryCache _cache;

        private const string ProductCacheKey = "GeminiProductContext";
        private const int MaxRetries = 3;
        private const int RetryDelayMs = 1000;

        public GeminiChatService(
            HttpClient httpClient,
            IProductService productService,
            IConfiguration config,
            ILogger<GeminiChatService> logger,
            IMemoryCache cache)
        {
            _httpClient = httpClient;
            _productService = productService;
            _config = config;
            _logger = logger;
            _cache = cache;
        }

        public async Task<string> GetAIResponseAsync(string userMessage, IEnumerable<ChatMessage> chatHistory)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                return "Vui lòng nhập câu hỏi.";

            var apiKey = _config["GeminiAI:ApiKey"];

            // Validate API key: must be present, not a placeholder, and match the Google API key format
            if (string.IsNullOrWhiteSpace(apiKey) ||
                apiKey == "ĐIỀN_API_KEY_CỦA_BẠN_VÀO_ĐÂY" ||
                !apiKey.StartsWith("AIzaSy") ||
                apiKey.Length < 30)
            {
                _logger.LogWarning("Gemini API Key không hợp lệ hoặc chưa được cấu hình. Key length: {Length}", apiKey?.Length ?? 0);
                return "Hệ thống AI chưa được cấu hình API Key hợp lệ. Vui lòng kiểm tra mục GeminiAI:ApiKey trong appsettings.json.";
            }

            _logger.LogInformation("Gemini API Key hợp lệ. Key length: {Length}", apiKey.Length);

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

            // Build chat history contents
            var contents = new List<object>();
            foreach (var msg in chatHistory.OrderBy(m => m.Timestamp))
            {
                contents.Add(new
                {
                    role = msg.IsFromAdmin ? "model" : "user",
                    parts = new[] { new { text = msg.Message } }
                });
            }

            // Add the current user message
            contents.Add(new
            {
                role = "user",
                parts = new[] { new { text = userMessage } }
            });

            // Build payload using the systemInstruction field so context is always present
            var systemInstruction = BuildSystemInstruction();
            var payload = new
            {
                systemInstruction = new
                {
                    parts = new[] { new { text = systemInstruction } }
                },
                contents = contents,
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 2048
                }
            };

            var jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            _logger.LogInformation("Gửi yêu cầu tới Gemini API. History: {HistoryCount} tin, Message length: {MsgLen}",
                chatHistory.Count(), userMessage.Length);

            // Retry loop for transient failures
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync(url, requestContent);
                    var responseString = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation("Gemini API phản hồi: {Status} (lần {Attempt}/{Max})",
                        response.StatusCode, attempt, MaxRetries);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = ParseGeminiResponse(responseString);
                        if (result != null)
                        {
                            _logger.LogInformation("Gemini phản hồi thành công. Độ dài: {Length}", result.Length);
                            return result;
                        }
                        _logger.LogWarning("Gemini phản hồi thành công nhưng không có nội dung text.");
                        return "AI không trả về nội dung. Vui lòng thử lại.";
                    }

                    _logger.LogError("Gemini API lỗi: {Status}. Body: {Body}", response.StatusCode, responseString);

                    // Do not retry on client errors (4xx) — they indicate a configuration issue
                    if ((int)response.StatusCode >= 400 && (int)response.StatusCode < 500)
                        return $"Lỗi cấu hình AI ({response.StatusCode}). Vui lòng kiểm tra API Key và cấu hình.";

                    // Retry on server errors (5xx) or rate-limiting (429)
                    if (attempt < MaxRetries)
                    {
                        _logger.LogWarning("Thử lại lần {Attempt}/{Max} sau {Delay}ms...", attempt + 1, MaxRetries, RetryDelayMs * attempt);
                        await Task.Delay(RetryDelayMs * attempt);
                        continue;
                    }

                    return $"Lỗi kết nối AI ({response.StatusCode}). Vui lòng thử lại sau.";
                }
                catch (TaskCanceledException ex)
                {
                    _logger.LogError(ex, "Timeout khi gọi Gemini API (lần {Attempt}/{Max})", attempt, MaxRetries);
                    if (attempt == MaxRetries)
                        return "Kết nối tới AI bị timeout. Vui lòng thử lại.";
                    await Task.Delay(RetryDelayMs * attempt);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "Lỗi HTTP khi gọi Gemini API (lần {Attempt}/{Max})", attempt, MaxRetries);
                    if (attempt == MaxRetries)
                        return "Lỗi kết nối mạng khi gọi AI. Vui lòng kiểm tra kết nối internet.";
                    await Task.Delay(RetryDelayMs * attempt);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi không xác định khi gọi Gemini API (lần {Attempt}/{Max})", attempt, MaxRetries);
                    return $"Lỗi không xác định: {ex.Message}";
                }
            }

            return "AI không phản hồi sau nhiều lần thử. Vui lòng thử lại sau.";
        }

        private string? ParseGeminiResponse(string responseString)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseString);
                var root = doc.RootElement;

                if (!root.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
                {
                    _logger.LogWarning("Gemini response không có candidates. Response: {Response}", responseString);
                    return null;
                }

                var firstCandidate = candidates[0];

                if (firstCandidate.TryGetProperty("finishReason", out var finishReason))
                {
                    var reason = finishReason.GetString();
                    if (reason == "SAFETY" || reason == "RECITATION")
                    {
                        _logger.LogWarning("Gemini từ chối phản hồi vì lý do an toàn. Finish reason: {Reason}", reason);
                        return "AI không thể trả lời câu hỏi này vì lý do an toàn nội dung.";
                    }
                }

                if (!firstCandidate.TryGetProperty("content", out var contentProp) ||
                    !contentProp.TryGetProperty("parts", out var parts) ||
                    parts.GetArrayLength() == 0 ||
                    !parts[0].TryGetProperty("text", out var textProp))
                {
                    _logger.LogWarning("Cấu trúc phản hồi Gemini không hợp lệ. Response: {Response}", responseString);
                    return null;
                }

                return textProp.GetString();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Không thể parse phản hồi Gemini: {Response}", responseString);
                return null;
            }
        }

        private string BuildSystemInstruction()
        {
            return _cache.GetOrCreate(ProductCacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                _logger.LogInformation("Đang xây dựng system instruction từ database (cache miss).");
                return BuildSystemInstructionFromDb();
            })!;
        }

        private string BuildSystemInstructionFromDb()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Bạn là trợ lý AI chuyên nghiệp của cửa hàng điện thoại HDKmall.");
            sb.AppendLine("Nhiệm vụ của bạn là tư vấn bán hàng, giới thiệu điện thoại, so sánh cấu hình và giải đáp thắc mắc cho khách hàng một cách lịch sự, thân thiện và chuyên nghiệp.");
            sb.AppendLine();
            sb.AppendLine("Khả năng của bạn:");
            sb.AppendLine("- Tư vấn sản phẩm phù hợp theo nhu cầu và ngân sách của khách hàng.");
            sb.AppendLine("- So sánh chi tiết thông số kỹ thuật (màn hình, chip, camera, pin, RAM, ROM, giá) giữa các mẫu điện thoại.");
            sb.AppendLine("- Giải thích các tính năng kỹ thuật một cách dễ hiểu cho người dùng phổ thông.");
            sb.AppendLine("- Cung cấp thông tin về giá cả, màu sắc và tình trạng hàng trong kho.");
            sb.AppendLine("- Trả lời câu hỏi về chính sách bảo hành, đổi trả của cửa hàng.");
            sb.AppendLine("- Sử dụng kiến thức cập nhật về công nghệ để tư vấn các sản phẩm mới nhất, review từ chuyên gia và xu hướng thị trường.");
            sb.AppendLine();
            sb.AppendLine("Thông tin cửa hàng HDKmall:");
            sb.AppendLine("- Chuyên kinh doanh điện thoại di động chính hãng.");
            sb.AppendLine("- Bảo hành chính hãng theo quy định của từng nhà sản xuất.");
            sb.AppendLine("- Hỗ trợ trả góp 0% lãi suất (điều kiện áp dụng theo chương trình).");
            sb.AppendLine("- Giao hàng toàn quốc, đổi trả trong 7 ngày nếu có lỗi từ nhà sản xuất.");
            sb.AppendLine();
            sb.AppendLine("Danh sách sản phẩm hiện có tại HDKmall:");
            sb.AppendLine("================================================================");

            try
            {
                var products = _productService.GetAllProducts()?.ToList();
                if (products != null && products.Any())
                {
                    foreach (var p in products)
                    {
                        sb.AppendLine($"[{p.Name}] — Giá từ: {p.Price:N0} VND");
                        if (p.Versions != null && p.Versions.Any())
                        {
                            foreach (var v in p.Versions)
                            {
                                sb.AppendLine($"  • Phiên bản: {v.Name} | Giá: {v.BasePrice:N0} VND");
                                if (v.Specifications != null && v.Specifications.Any())
                                {
                                    sb.AppendLine("    Thông số kỹ thuật:");
                                    foreach (var s in v.Specifications)
                                        sb.AppendLine($"      - {s.SpecName}: {s.SpecValue}");
                                }
                                if (v.Variants != null && v.Variants.Any())
                                {
                                    var colors = string.Join(", ", v.Variants.Select(vr => vr.Color));
                                    sb.AppendLine($"    Màu sắc có sẵn: {colors}");
                                }
                            }
                        }
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.AppendLine("(Hiện chưa có dữ liệu sản phẩm trong hệ thống)");
                    sb.AppendLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải dữ liệu sản phẩm từ database.");
                sb.AppendLine("(Không thể tải dữ liệu sản phẩm lúc này — vẫn có thể tư vấn chung)");
                sb.AppendLine();
            }

            sb.AppendLine("================================================================");
            sb.AppendLine();
            sb.AppendLine("Hướng dẫn trả lời:");
            sb.AppendLine("1. Luôn trả lời bằng tiếng Việt, lịch sự và chuyên nghiệp.");
            sb.AppendLine("2. Ưu tiên giới thiệu các sản phẩm có trong danh sách trên khi khách muốn mua hàng tại HDKmall.");
            sb.AppendLine("3. Nếu khách hỏi về sản phẩm không có trong shop, hãy cung cấp thông tin từ kiến thức của bạn, sau đó khéo léo giới thiệu sản phẩm tương đương tại HDKmall.");
            sb.AppendLine("4. Khi so sánh điện thoại, trình bày rõ ràng theo từng tiêu chí (màn hình, chip xử lý, camera, dung lượng pin, giá cả).");
            sb.AppendLine("5. Nếu không chắc chắn về thông tin, hãy nói rõ và khuyến khích khách đến cửa hàng để được tư vấn trực tiếp.");
            sb.AppendLine("6. Có thể dùng emoji phù hợp để câu trả lời sinh động, dễ đọc hơn.");

            return sb.ToString();
        }
    }
}
