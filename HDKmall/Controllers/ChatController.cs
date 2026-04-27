using System;
using System.Security.Claims;
using System.Threading.Tasks;
using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HDKmall.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IGeminiChatService _geminiChatService;

        public ChatController(IChatService chatService, IGeminiChatService geminiChatService)
        {
            _chatService = chatService;
            _geminiChatService = geminiChatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return BadRequest();

            var history = await _chatService.GetChatHistoryAsync(userId);
            await _chatService.MarkAsReadAsync(userId, false);
            return Json(history);
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return BadRequest();

            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return BadRequest();

            var chatMsg = new ChatMessage
            {
                UserId = userId,
                SenderId = userId,
                Message = message,
                IsFromAdmin = false,
                Timestamp = DateTime.Now
            };

            var result = await _chatService.SendMessageAsync(chatMsg);
            return Json(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> AskAI([FromBody] string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return BadRequest();

            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return BadRequest();

            // 1. Lấy lịch sử chat (để AI có ngữ cảnh trò chuyện)
            // Lấy 10 tin nhắn gần nhất để khỏi tràn context
            var rawHistory = await _chatService.GetChatHistoryAsync(userId);
            var recentHistory = rawHistory.TakeLast(10);

            // 2. Gửi vào Database là User nhắn (lưu log)
            var userMsg = new ChatMessage
            {
                UserId = userId,
                SenderId = userId,
                Message = message,
                IsFromAdmin = false,
                Timestamp = DateTime.Now
            };
            await _chatService.SendMessageAsync(userMsg);

            // 3. Gọi Gemini AI
            var aiResponseText = await _geminiChatService.GetAIResponseAsync(message, recentHistory);

            // 4. Lưu câu trả lời của AI vào Database (giả lập Admin trả lời)
            var aiMsg = new ChatMessage
            {
                UserId = userId,
                SenderId = 0, // 0 = Admin/System
                Message = aiResponseText,
                IsFromAdmin = true,
                Timestamp = DateTime.Now
            };
            await _chatService.SendMessageAsync(aiMsg);

            return Json(new { success = true, reply = aiResponseText });
        }
    }
}
