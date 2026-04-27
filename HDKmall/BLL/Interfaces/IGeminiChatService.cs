using System.Collections.Generic;
using System.Threading.Tasks;

namespace HDKmall.BLL.Interfaces
{
    public interface IGeminiChatService
    {
        // Nhận vào tin nhắn khách hàng và dữ liệu sản phẩm tìm được để trả lời
        Task<string> GetAIResponseAsync(string userMessage, string productContext);
    }
}
