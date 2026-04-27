using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDKmall.BLL.Interfaces;
using HDKmall.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HDKmall.Controllers
{
    public class ChatAIController : Controller
    {
        private readonly IGeminiChatService _geminiService;
        private readonly ApplicationDbContext _context;

        public ChatAIController(IGeminiChatService geminiService, ApplicationDbContext context)
        {
            _geminiService = geminiService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Message))
                return Json(new { reply = "Dạ, anh/chị cần em hỗ trợ gì ạ?" });

            try
            {
                var keywords = request.Message
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Where(k => k.Length > 1)
                    .ToList();

                // Query sản phẩm kèm Versions và Variants để lấy đúng giá
                var relatedProducts = await _context.Products
                    .Include(p => p.Versions)
                        .ThenInclude(v => v.Variants)
                    .Where(p => keywords.Any(k => p.Name.Contains(k)))
                    .Take(8)
                    .ToListAsync();

                // Nếu không tìm được theo từ khoá → lấy tất cả sản phẩm có giá
                if (!relatedProducts.Any())
                {
                    relatedProducts = await _context.Products
                        .Include(p => p.Versions)
                            .ThenInclude(v => v.Variants)
                        .OrderBy(p => p.Name)
                        .Take(20)
                        .ToListAsync();
                }

                // Xây dựng context chỉ chứa sản phẩm có giá thật
                var contextBuilder = new StringBuilder();
                foreach (var product in relatedProducts)
                {
                    var lines = BuildProductLines(product);
                    foreach (var line in lines)
                        contextBuilder.AppendLine(line);
                }

                var productContext = contextBuilder.Length > 0
                    ? contextBuilder.ToString()
                    : "Shop hiện có nhiều dòng điện thoại, loa, phụ kiện từ các thương hiệu uy tín.";

                var reply = await _geminiService.GetAIResponseAsync(request.Message, productContext);
                return Json(new { reply = reply });
            }
            catch (Exception)
            {
                return Json(new { reply = "Dạ, hệ thống tư vấn bên em đang bận một chút, anh/chị đợi em tí nhé!" });
            }
        }

        /// <summary>
        /// Tạo danh sách dòng mô tả giá cho một sản phẩm.
        /// Ưu tiên: Version.BasePrice > Variant.Price > Product.Price
        /// </summary>
        private static List<string> BuildProductLines(Product product)
        {
            var lines = new List<string>();

            if (product.Versions != null && product.Versions.Any())
            {
                foreach (var version in product.Versions)
                {
                    if (version.BasePrice > 0)
                    {
                        // Trường hợp 1: Điện thoại có Version với giá theo dung lượng
                        // VD: iPhone 15 Pro Max (256GB): 34,990,000 VND
                        lines.Add($"- {product.Name} ({version.Name}): {version.BasePrice:N0} VND");
                    }
                    else if (version.Variants != null && version.Variants.Any(v => v.Price > 0))
                    {
                        // Trường hợp 2: Sản phẩm chỉ có màu sắc, giá theo màu
                        // VD: Loa Soundcore (Màu Đen): 1,090,000 VND
                        var variants = version.Variants.Where(v => v.Price > 0).ToList();

                        // Kiểm tra nếu tất cả màu cùng giá → gộp lại cho gọn
                        if (variants.Select(v => v.Price).Distinct().Count() == 1)
                        {
                            lines.Add($"- {product.Name}: {variants.First().Price:N0} VND (nhiều màu: {string.Join(", ", variants.Select(v => v.Color))})");
                        }
                        else
                        {
                            // Màu khác giá → liệt kê từng màu
                            foreach (var variant in variants.OrderBy(v => v.Price))
                                lines.Add($"- {product.Name} ({variant.Color}): {variant.Price:N0} VND");
                        }
                    }
                }
            }

            // Trường hợp 3: Sản phẩm không có Version, có giá trực tiếp ở Product
            if (lines.Count == 0 && product.Price > 0)
            {
                lines.Add($"- {product.Name}: {product.Price:N0} VND");
            }

            // Nếu không có giá nào hợp lệ → bỏ qua hoàn toàn (không thêm gì)
            return lines;
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
    }
}
