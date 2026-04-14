using HDKmall.BLL.Interfaces;
using HDKmall.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HDKmall.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IVNPayService _vnPayService;
        private readonly IMoMoService _moMoService;
        private readonly IPaymentService _paymentService;

        public PaymentController(
            IOrderService orderService,
            IVNPayService vnPayService,
            IMoMoService moMoService,
            IPaymentService paymentService)
        {
            _orderService = orderService;
            _vnPayService = vnPayService;
            _moMoService = moMoService;
            _paymentService = paymentService;
        }

        // GET: Payment/Index (Choose payment method)
        public IActionResult Index(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (order.UserId != userId && !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            return View(order);
        }

        // POST: Payment/ProcessPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(int orderId, string paymentMethod)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (order.UserId != userId && !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            var paymentModel = new PaymentVM
            {
                OrderId = orderId,
                TotalAmount = order.TotalAmount,
                PaymentMethod = paymentMethod,
                OrderCode = $"ORDER-{orderId}-{DateTime.Now.Ticks}",
                ReturnUrl = Url.Action("PaymentCallback", "Payment", new { orderId }, Request.Scheme),
                IpnUrl = Url.Action("VNPayIPN", "Payment", null, Request.Scheme)
            };

            _paymentService.CreatePayment(orderId, paymentMethod, order.TotalAmount);

            switch (paymentMethod)
            {
                case "COD":
                    // Cash on Delivery - không cần chuyển hướng, chỉ cập nhật trạng thái
                    _orderService.UpdateOrderStatus(orderId, "Processing");
                    TempData["Success"] = "Đơn hàng của bạn đã được tạo. Vui lòng thanh toán khi nhận hàng.";
                    return RedirectToAction("Detail", "Order", new { id = orderId });

                case "VNPay":
                    var vnPayUrl = _vnPayService.CreatePaymentUrl(paymentModel, HttpContext);
                    return Redirect(vnPayUrl);

                case "MoMo":
                    try
                    {
                        var momoUrl = await _moMoService.CreatePaymentUrl(paymentModel, HttpContext);
                        if (string.IsNullOrEmpty(momoUrl))
                        {
                            TempData["Error"] = "Lỗi kết nối MoMo. Vui lòng thử lại.";
                            return RedirectToAction("Index", new { orderId });
                        }
                        return Redirect(momoUrl);
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = $"Lỗi: {ex.Message}";
                        return RedirectToAction("Index", new { orderId });
                    }

                default:
                    TempData["Error"] = "Phương thức thanh toán không hợp lệ";
                    return RedirectToAction("Index", new { orderId });
            }
        }

        // GET: Payment/PaymentCallback (VNPay & MoMo callback)
        [AllowAnonymous]
        public async Task<IActionResult> PaymentCallback(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }

            // Determine payment method từ order hoặc query params
            var paymentMethod = Request.Query["PaymentMethod"].ToString();

            VNPaymentResponseVM response = null;

            // Try VNPay callback
            if (Request.Query.Any(x => x.Key.StartsWith("vnp_")))
            {
                response = _vnPayService.PaymentExecute(Request.Query);
                paymentMethod = "VNPay";
            }
            // Try MoMo callback
            else if (Request.Query.Any(x => x.Key == "resultCode"))
            {
                response = await _moMoService.PaymentExecute(Request.Query);
                paymentMethod = "MoMo";
            }

            if (response != null && response.Success)
            {
                _orderService.UpdateOrderStatus(orderId, "Processing");
                _paymentService.UpdatePaymentStatus(orderId, "Success", response.TransactionId);
                TempData["Success"] = "Thanh toán thành công! Đơn hàng đang được xử lý.";
                return RedirectToAction("Detail", "Order", new { id = orderId });
            }
            else
            {
                TempData["Error"] = response?.Message ?? "Thanh toán thất bại. Vui lòng thử lại.";
                return RedirectToAction("Index", new { orderId });
            }
        }

        // POST: Payment/VNPayIPN (VNPay IPN endpoint)
        [AllowAnonymous]
        [HttpPost]
        public IActionResult VNPayIPN()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.Success)
            {
                var order = _orderService.GetOrderById(response.OrderId);
                if (order != null)
                {
                    _orderService.UpdateOrderStatus(response.OrderId, "Processing");
                    _paymentService.UpdatePaymentStatus(response.OrderId, "Success", response.TransactionId);
                    return Json(new { RspCode = "00", Message = "Cập nhật thành công" });
                }
            }

            return Json(new { RspCode = "01", Message = "Lỗi cập nhật" });
        }

        // GET: Payment/History
        public IActionResult History()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var orders = _orderService.GetUserOrders(userId);

            return View(orders);
        }

        // GET: Payment/Receipt
        public IActionResult Receipt(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (order.UserId != userId && !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            var paymentHistory = _paymentService.GetPaymentHistory(orderId);
            ViewBag.PaymentHistory = paymentHistory;

            return View(order);
        }
    }
}
