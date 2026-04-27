using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HDKmall.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HDKmall.BLL.Services
{
    public class GeminiChatService : IGeminiChatService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<GeminiChatService> _logger;

        public GeminiChatService(HttpClient httpClient, IConfiguration config, ILogger<GeminiChatService> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
        }

        public async Task<string> GetAIResponseAsync(string userMessage, string productContext)
        {
            var apiKey = _config["Gemini:ApiKey"]?.Trim();
            if (string.IsNullOrEmpty(apiKey))
                return "Dạ, em chưa được cấu hình API Key ạ.";

            // BƯỚC 1: Hỏi Google xem API Key này được dùng model nào
            var availableModels = await GetSupportedGenerateModelsAsync(apiKey);

            if (!availableModels.Any())
            {
                return "Dạ, API Key này không có model nào khả dụng. Vui lòng kiểm tra tại aistudio.google.com ạ.";
            }

            var systemPrompt = BuildSystemPrompt(productContext);

            // BƯỚC 2: Thử từng model khả dụng theo thứ tự ưu tiên
            var preferred = new[] { "gemini-2.0-flash", "gemini-1.5-flash", "gemini-2.0-flash-lite", "gemini-1.0-pro", "gemini-pro" };
            
            // Sắp xếp: model ưa thích trước, model khác sau
            var orderedModels = availableModels
                .OrderBy(m => {
                    var idx = Array.FindIndex(preferred, p => m.Contains(p, StringComparison.OrdinalIgnoreCase));
                    return idx >= 0 ? idx : 999;
                })
                .ToList();

            _logger.LogInformation("Models khả dụng: {Models}", string.Join(", ", orderedModels));

            foreach (var modelFullName in orderedModels)
            {
                // modelFullName dạng "models/gemini-2.0-flash" → ta lấy phần sau "models/"
                var modelId = modelFullName.StartsWith("models/") ? modelFullName.Substring(7) : modelFullName;

                var result = await TryCallModel(modelId, "v1beta", userMessage, systemPrompt, apiKey);
                if (result != null) return result;
            }

            return "Dạ, hiện tại em chưa thể kết nối với AI. Anh/chị vui lòng thử lại sau nhé!";
        }

        // Lấy danh sách model hỗ trợ generateContent từ API Key hiện tại
        private async Task<List<string>> GetSupportedGenerateModelsAsync(string apiKey)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models?key={apiKey}";
            var result = new List<string>();
            try
            {
                var response = await _httpClient.GetAsync(url);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("ListModels thất bại: {Status} - {Body}", response.StatusCode, body);
                    return result;
                }

                using var doc = JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("models", out var models))
                {
                    foreach (var m in models.EnumerateArray())
                    {
                        // Chỉ lấy model hỗ trợ generateContent
                        var name = m.GetProperty("name").GetString();
                        var supportedMethods = m.TryGetProperty("supportedGenerationMethods", out var methods)
                            ? methods.EnumerateArray().Select(x => x.GetString()).ToList()
                            : new List<string?>();

                        if (supportedMethods.Contains("generateContent") && name != null)
                        {
                            result.Add(name);
                            _logger.LogInformation("✅ Model khả dụng: {Name}", name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi ListModels API");
            }
            return result;
        }

        private async Task<string?> TryCallModel(string modelId, string apiVersion, string userMessage, string systemPrompt, string apiKey)
        {
            var url = $"https://generativelanguage.googleapis.com/{apiVersion}/models/{modelId}:generateContent?key={apiKey}";

            var payload = new
            {
                systemInstruction = new { parts = new[] { new { text = systemPrompt } } },
                contents = new[] { new { role = "user", parts = new[] { new { text = userMessage } } } },
                generationConfig = new { temperature = 0.7, maxOutputTokens = 1024 }
            };

            try
            {
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Thành công: model={ModelId}", modelId);
                    using var doc = JsonDocument.Parse(responseBody);
                    return doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString() ?? "Dạ, em không có câu trả lời ạ.";
                }
                else
                {
                    _logger.LogWarning("❌ model={ModelId} => {Status}", modelId, (int)response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi gọi model={ModelId}", modelId);
                return null;
            }
        }

        private static string BuildSystemPrompt(string productContext)
        {
            return "Bạn là chuyên gia tư vấn bán hàng của HDKmall - cửa hàng điện thoại và công nghệ.\n" +
                   "PHONG CÁCH: Nhiệt tình, thân thiện, luôn xưng 'Dạ' và 'Em'.\n" +
                   "QUY TẮC:\n" +
                   "1. Ưu tiên dùng thông tin sản phẩm bên dưới để tư vấn chính xác giá và tên máy.\n" +
                   "2. Nếu sản phẩm không có trong danh sách, dùng kiến thức công nghệ để trả lời rồi giới thiệu máy tương đương ở shop.\n" +
                   "3. Từ chối lịch sự các câu hỏi không liên quan đến công nghệ/mua sắm.\n" +
                   "4. Trình bày rõ ràng, dùng gạch đầu dòng.\n\n" +
                   "DANH SÁCH SẢN PHẨM TẠI SHOP:\n" +
                   "========================\n" +
                   productContext +
                   "\n========================";
        }
    }
}
