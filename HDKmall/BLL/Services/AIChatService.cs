using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HDKmall.BLL.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HDKmall.BLL.Services
{
    public class AIChatService : IAIChatService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AIChatService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiKey = configuration["GeminiAI:ApiKey"] ?? "";
        }

        public async Task<string> GetResponseAsync(string userMessage, string context)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                return "Dạ, hiện tại hệ thống AI đang bảo trì. Anh/Chị vui lòng liên hệ trực tiếp với Admin để được hỗ trợ nhé!";
            }

            try
            {
               var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = $"{context}\n\nNgười dùng hỏi: {userMessage}" }
                            }
                        }
                    }
                };

                var jsonRequest = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorDetail = await response.Content.ReadAsStringAsync();
                    // Log lỗi ra console để Admin có thể kiểm tra
                    Console.WriteLine($"Gemini API Error: {response.StatusCode} - {errorDetail}");
                    
                    return "Dạ, em gặp một chút trục trặc khi kết nối với hệ thống tư vấn (Lỗi: " + response.StatusCode + "). Anh/Chị thử lại sau giây lát nhé!";
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);
                
                // Bóc tách JSON theo cấu trúc của Gemini API
                if (doc.RootElement.TryGetProperty("candidates", out var candidates) &&
                    candidates.GetArrayLength() > 0 &&
                    candidates[0].TryGetProperty("content", out var resContent) &&
                    resContent.TryGetProperty("parts", out var parts) &&
                    parts.GetArrayLength() > 0 &&
                    parts[0].TryGetProperty("text", out var text))
                {
                    return text.GetString() ?? "Dạ, em chưa tìm được câu trả lời phù hợp. Anh/Chị có thể hỏi cụ thể hơn không ạ?";
                }

                return "Dạ, em xin lỗi nhưng em chưa hiểu ý Anh/Chị lắm. Anh/Chị có thể nhắc lại được không ạ?";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AIChatService: {ex.Message}");
                return "Dạ, hệ thống đang bận một chút. Anh/Chị vui lòng đợi giây lát hoặc chat với Admin nhé!";
            }
        }
    }
}
