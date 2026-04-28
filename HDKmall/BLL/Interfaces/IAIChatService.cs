using System.Threading.Tasks;

namespace HDKmall.BLL.Interfaces
{
    public interface IAIChatService
    {
       /// <summary>
       /// Gửi câu hỏi của người dùng và ngữ cảnh sản phẩm tới AI để nhận phản hồi.
       /// </summary>
       /// <param name="userMessage">Tin nhắn từ người dùng</param>
       /// <param name="context">Thông tin sản phẩm, khuyến mãi từ Database</param>
       /// <returns>Câu trả lời từ AI</returns>
        Task<string> GetResponseAsync(string userMessage, string context);
    }
}
