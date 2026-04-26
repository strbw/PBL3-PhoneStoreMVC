using System.Collections.Generic;
using System.Threading.Tasks;
using HDKmall.Models;

namespace HDKmall.BLL.Interfaces
{
    public interface IChatService
    {
        Task<bool> SendMessageAsync(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int userId);
        Task<IEnumerable<User>> GetActiveChatUsersAsync();
        Task<int> GetUnreadCountAsync(int userId, bool forAdmin);
        Task MarkAsReadAsync(int userId, bool forAdmin);
    }
}
