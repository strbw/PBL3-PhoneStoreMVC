using System.Collections.Generic;
using System.Threading.Tasks;
using HDKmall.Models;

namespace HDKmall.BLL.Interfaces
{
    public interface IGeminiChatService
    {
        Task<string> GetAIResponseAsync(string userMessage, IEnumerable<ChatMessage> chatHistory);
    }
}
