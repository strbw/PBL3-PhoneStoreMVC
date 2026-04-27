using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HDKmall.BLL.Interfaces;
using HDKmall.Models;
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

        public GeminiChatService(HttpClient httpClient, IProductService productService, IConfiguration config, ILogger<GeminiChatService> logger)
        {
            _httpClient = httpClient;
            _productService = productService;
            _config = config;
            _logger = logger;
        }

        public async Task<string> GetAIResponseAsync(string userMessage, IEnumerable<ChatMessage> chatHistory)
        {
            var apiKey = _config["GeminiAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey) || apiKey == "ĐIỀN_API_KEY_CỦA_BẠN_VÀO_ĐÂY")
            {
                return "Hệ thống AI chưa được cấu hình API Key. Vui lòng điền Key vào appsettings.json.";
            }

            // Log độ dài Key để kiểm tra (không log key thật)
            _logger.LogInformation("Gemini API Key length: {Length}", apiKey?.Length ?? 0);

            // Dùng v1beta + gemini-1.5-flash + key trong URL (Cách "cổ điển" nhất, tỉ lệ thành công cao nhất)
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";
            
            // 1. Lấy dữ liệu cửa hàng
            var shopContext = BuildSystemInstruction();

            // 2. Format Chat History
            var contents = new List<object>();
            
            // Nếu là tin nhắn đầu tiên, ta chèn ngữ cảnh của shop vào để AI hiểu nó là ai
            string promptPrefix = chatHistory.Any() ? "" : shopContext + "\n\n";

            foreach (var msg in chatHistory.OrderBy(m => m.Timestamp))
            {
                contents.Add(new
                {
                    role = msg.IsFromAdmin ? "model" : "user",
                    parts = new[] { new { text = msg.Message } }
                });
            }

            // Tin nhắn hiện tại của User
            contents.Add(new
            {
                role = "user",
                parts = new[] { new { text = promptPrefix + userMessage } }
            });

            // 3. Payload
            var payload = new
            {
                contents = contents,
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 1000
                }
            };

            var jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            try
            {
                var response = await _httpClient.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(responseString);
                    var textResponse = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text").GetString();

                    return textResponse ?? "AI không phản hồi.";
                }
                else
                {
                    _logger.LogError("Gemini API Error: {Status}. Body: {Body}", response.StatusCode, responseString);
                    return $"Lỗi AI ({response.StatusCode}): {responseString}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception calling Gemini API");
                return $"Lỗi kết nối: {ex.Message}";
            }
        }

        private string BuildSystemInstruction()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Bạn là trợ lý AI chuyên nghiệp của cửa hàng điện thoại HDKmall.");
            sb.AppendLine("Nhiệm vụ của bạn là tư vấn bán hàng, giới thiệu điện thoại, so sánh cấu hình và giải đáp thắc mắc cho khách hàng một cách lịch sự, thân thiện.");
            sb.AppendLine("Bạn được cấp quyền truy cập Google Search để lấy thêm thông tin (vd: review, so sánh) từ mạng nếu cần.");
            sb.AppendLine("Dưới đây là danh sách các sản phẩm đang có bán tại HDKmall. Chỉ tư vấn các sản phẩm này nếu khách muốn mua tại shop:");
            sb.AppendLine("--------------------------------");

            var products = _productService.GetAllProducts();
            foreach (var p in products)
            {
                sb.AppendLine($"- Sản phẩm: {p.Name} (Giá từ: {p.Price:N0} VND)");
                if (p.Versions != null && p.Versions.Any())
                {
                    foreach (var v in p.Versions)
                    {
                        sb.AppendLine($"  + Phiên bản: {v.Name} - Giá: {v.BasePrice:N0} VND");
                        
                        // Thông số kỹ thuật
                        if (v.Specifications != null && v.Specifications.Any())
                        {
                            var specs = string.Join(", ", v.Specifications.Select(s => $"{s.SpecName}: {s.SpecValue}"));
                            sb.AppendLine($"    * Thông số: {specs}");
                        }

                        // Màu sắc
                        if (v.Variants != null && v.Variants.Any())
                        {
                            var colors = string.Join(", ", v.Variants.Select(vr => vr.Color));
                            sb.AppendLine($"    * Màu sắc có sẵn: {colors}");
                        }
                    }
                }
                sb.AppendLine();
            }

            sb.AppendLine("--------------------------------");
            sb.AppendLine("Luôn trả lời bằng tiếng Việt. Nếu khách hỏi về sản phẩm không có trong danh sách trên, hãy dùng Google Search để tư vấn nhưng khéo léo giới thiệu các mẫu tương đương đang có tại shop.");

            return sb.ToString();
        }
    }
}
