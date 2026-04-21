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
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IOrderService orderService,
            IVNPayService vnPayService,
            IMoMoService moMoService,
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _orderService = orderService;
            _vnPayService = vnPayService;
            _moMoService = moMoService;
            _paymentService = paymentService;
            _logger = logger;
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

            _paymentService.CreatePayment(orderId, paymentMethod, order.TotalAmount);

            _logger.LogInformation("Processing payment for order {OrderId} via {PaymentMethod}", orderId, paymentMethod);

            switch (paymentMethod)
            {
                case "COD":
                    // Cash on Delivery – cập nhật trạng thái sang Processing ngay
                    _orderService.UpdateOrderStatus(orderId, "Processing");
                    _logger.LogInformation("COD order {OrderId} set to Processing", orderId);
                    TempData["Success"] = "Đơn hàng của bạn đã được tạo. Vui lòng thanh toán khi nhận hàng.";
                    return RedirectToAction("Detail", "Order", new { id = orderId });

                case "VNPay":
                    // Dùng endpoint riêng /payment/vnpay-return để xử lý kết quả
                    var vnPayReturnUrl = Url.Action("VnPayReturn", "Payment", new { orderId }, Request.Scheme);
                    var vnPayModel = new PaymentVM
                    {
                        OrderId = orderId,
                        TotalAmount = order.TotalAmount,
                        PaymentMethod = paymentMethod,
                        OrderCode = $"ORDER-{orderId}-{DateTime.Now.Ticks}",
                        ReturnUrl = vnPayReturnUrl
                    };
                    var vnPayUrl = _vnPayService.CreatePaymentUrl(vnPayModel, HttpContext);
                    _logger.LogInformation("Redirecting order {OrderId} to VNPay", orderId);
                    return Redirect(vnPayUrl);

                case "MoMo":
                    // Dùng endpoint riêng /payment/momo-return để xử lý kết quả
                    // ta sẽ parse orderId từ OrderCode khi callback
                    var momoReturnUrl = Url.Action("MoMoReturn", "Payment", null, Request.Scheme);
                    var momoIpnUrl = Url.Action("MoMoIPN", "Payment", null, Request.Scheme) ?? momoReturnUrl;
                    var momoModel = new PaymentVM
                    {
                        OrderId = orderId,
                        TotalAmount = order.TotalAmount,
                        PaymentMethod = paymentMethod,
                        OrderCode = $"ORDER-{orderId}-{DateTime.Now.Ticks}",
                        ReturnUrl = momoReturnUrl,
                        IpnUrl = momoIpnUrl
                    };
                    try
                    {
                        var momoUrl = await _moMoService.CreatePaymentUrl(momoModel, HttpContext);
                        if (string.IsNullOrEmpty(momoUrl))
                        {
                            TempData["Error"] = "Lỗi kết nối MoMo. Vui lòng thử lại.";
                            return RedirectToAction("Index", new { orderId });
                        }
                        _logger.LogInformation("Redirecting order {OrderId} to MoMo", orderId);
                        return Redirect(momoUrl);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error creating MoMo payment for order {OrderId}", orderId);
                        TempData["Error"] = $"Lỗi: {ex.Message}";
                        return RedirectToAction("Index", new { orderId });
                    }

                default:
                    TempData["Error"] = "Phương thức thanh toán không hợp lệ";
                    return RedirectToAction("Index", new { orderId });
            }
        }

        // GET: /payment/vnpay-return
        // VNPay redirect user về sau khi thanh toán (Return URL)
        [AllowAnonymous]
        [HttpGet]
        [Route("payment/vnpay-return")]
        public IActionResult VnPayReturn(int orderId)
        {
            _logger.LogInformation("VNPay return callback received for order {OrderId}", orderId);

            if (orderId == 0)
            {
                _logger.LogWarning("VNPay return: orderId is 0, cannot identify order");
                TempData["Error"] = "Không xác định được đơn hàng.";
                return RedirectToAction("Index", "Home");
            }

            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                _logger.LogWarning("VNPay return: order {OrderId} not found", orderId);
                return NotFound();
            }

            // Verify chữ ký VNPay (HMAC SHA512)
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (!response.Success)
            {
                _logger.LogWarning("VNPay return: signature invalid or payment failed for order {OrderId}. Message: {Message}", orderId, response.Message);
                _orderService.UpdateOrderStatus(orderId, "Failed");
                _paymentService.UpdatePaymentStatus(orderId, "Failed", response.TransactionId ?? "");
                TempData["Error"] = "Thanh toán VNPay thất bại hoặc chữ ký không hợp lệ.";
                return RedirectToAction("Detail", "Order", new { id = orderId });
            }

            // Cross-check số tiền: vnp_Amount (đơn vị xu) / 100 phải khớp order.TotalAmount
            if (response.Amount != order.TotalAmount)
            {
                _logger.LogWarning(
                    "VNPay return: amount mismatch for order {OrderId}. Expected {Expected}, got {Got}",
                    orderId, order.TotalAmount, response.Amount);
                _orderService.UpdateOrderStatus(orderId, "Failed");
                _paymentService.UpdatePaymentStatus(orderId, "Failed", response.TransactionId ?? "");
                TempData["Error"] = "Số tiền thanh toán không khớp. Vui lòng liên hệ hỗ trợ.";
                return RedirectToAction("Detail", "Order", new { id = orderId });
            }

            // Tránh cập nhật lại nếu đơn đã được xử lý trước đó
            if (order.Status != "Pending")
            {
                _logger.LogInformation("VNPay return: order {OrderId} already in status {Status}, skipping update", orderId, order.Status);
                TempData["Success"] = "Đơn hàng đã được xử lý trước đó.";
                return RedirectToAction("Detail", "Order", new { id = orderId });
            }

            // Thành công – chuyển đơn sang Processing
            _orderService.UpdateOrderStatus(orderId, "Processing");
            _paymentService.UpdatePaymentStatus(orderId, "Success", response.TransactionId);
            _logger.LogInformation("VNPay payment successful for order {OrderId}, transactionId={TransactionId}", orderId, response.TransactionId);
            TempData["Success"] = "Thanh toán VNPay thành công! Đơn hàng đang được xử lý.";
            return RedirectToAction("Detail", "Order", new { id = orderId });
        }

        // GET: /payment/momo-return
        // MoMo redirect user về sau khi thanh toán (Return URL)
        [AllowAnonymous]
        [HttpGet]
        [Route("payment/momo-return")]
        public async Task<IActionResult> MoMoReturn()
        {
            // MoMo gửi orderId theo định dạng OrderCode: "ORDER-{id}-{ticks}"
            var momoOrderId = Request.Query["orderId"].ToString();
            _logger.LogInformation("MoMo return callback received, momoOrderId={MoMoOrderId}", momoOrderId);

            // Parse orderId thực (DB id) từ OrderCode
            int orderId = 0;
            var parts = momoOrderId.Split('-');
            if (parts.Length >= 2)
            {
                int.TryParse(parts[1], out orderId);
            }

            if (orderId == 0)
            {
                _logger.LogWarning("MoMo return: cannot parse orderId from '{MoMoOrderId}'", momoOrderId);
                TempData["Error"] = "Không xác định được đơn hàng từ callback MoMo.";
                return RedirectToAction("Index", "Home");
            }

            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                _logger.LogWarning("MoMo return: order {OrderId} not found", orderId);
                return NotFound();
            }

            // Verify chữ ký MoMo (HMAC SHA256) và kiểm tra resultCode
            var response = await _moMoService.PaymentExecute(Request.Query);

            if (!response.Success)
            {
                _logger.LogWarning("MoMo return: payment failed for order {OrderId}. Message: {Message}", orderId, response.Message);
                _orderService.UpdateOrderStatus(orderId, "Failed");
                _paymentService.UpdatePaymentStatus(orderId, "Failed", response.TransactionId ?? "");
                TempData["Error"] = response.Message ?? "Thanh toán MoMo thất bại.";
                return RedirectToAction("Detail", "Order", new { id = orderId });
            }

            // Cross-check số tiền: response.Amount (đã chia 1000) phải khớp order.TotalAmount
            if (response.Amount != order.TotalAmount)
            {
                _logger.LogWarning(
                    "MoMo return: amount mismatch for order {OrderId}. Expected {Expected}, got {Got}",
                    orderId, order.TotalAmount, response.Amount);
                _orderService.UpdateOrderStatus(orderId, "Failed");
                _paymentService.UpdatePaymentStatus(orderId, "Failed", response.TransactionId ?? "");
                TempData["Error"] = "Số tiền thanh toán không khớp. Vui lòng liên hệ hỗ trợ.";
                return RedirectToAction("Detail", "Order", new { id = orderId });
            }

            // Tránh cập nhật lại nếu đơn đã được xử lý trước đó
            if (order.Status != "Pending")
            {
                _logger.LogInformation("MoMo return: order {OrderId} already in status {Status}, skipping update", orderId, order.Status);
                TempData["Success"] = "Đơn hàng đã được xử lý trước đó.";
                return RedirectToAction("Detail", "Order", new { id = orderId });
            }

            // Thành công – chuyển đơn sang Processing
            _orderService.UpdateOrderStatus(orderId, "Processing");
            _paymentService.UpdatePaymentStatus(orderId, "Success", response.TransactionId);
            _logger.LogInformation("MoMo payment successful for order {OrderId}, transactionId={TransactionId}", orderId, response.TransactionId);
            TempData["Success"] = "Thanh toán MoMo thành công! Đơn hàng đang được xử lý.";
            return RedirectToAction("Detail", "Order", new { id = orderId });
        }

        // GET: Payment/PaymentCallback (deprecated – giữ lại để tương thích ngược)
        // Redirect sang VnPayReturn hoặc MoMoReturn tuỳ params
        [AllowAnonymous]
        public async Task<IActionResult> PaymentCallback(int orderId)
        {
            _logger.LogWarning("Deprecated PaymentCallback hit for order {OrderId}; routing to specific handler", orderId);

            if (Request.Query.Any(x => x.Key.StartsWith("vnp_")))
            {
                // Redirect sang VnPayReturn, giữ nguyên toàn bộ query string
                return RedirectToAction("VnPayReturn", new { orderId });
            }
            else if (Request.Query.Any(x => x.Key == "resultCode"))
            {
                return await MoMoReturn();
            }

            TempData["Error"] = "Không xác định được phương thức thanh toán.";
            return RedirectToAction("Detail", "Order", new { id = orderId });
        }

        // POST: Payment/VNPayIPN (VNPay IPN endpoint – server-to-server, không dùng trong demo local)
        [AllowAnonymous]
        [HttpPost]
        public IActionResult VNPayIPN()
        {
            _logger.LogInformation("VNPay IPN received");
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.Success)
            {
                var order = _orderService.GetOrderById(response.OrderId);
                if (order != null)
                {
                    _orderService.UpdateOrderStatus(response.OrderId, "Processing");
                    _paymentService.UpdatePaymentStatus(response.OrderId, "Success", response.TransactionId);
                    _logger.LogInformation("VNPay IPN: order {OrderId} updated to Processing", response.OrderId);
                    return Json(new { RspCode = "00", Message = "Cập nhật thành công" });
                }
            }

            _logger.LogWarning("VNPay IPN: update failed for orderId={OrderId}", response.OrderId);
            return Json(new { RspCode = "01", Message = "Lỗi cập nhật" });
        }

        // POST: Payment/MoMoIPN (MoMo IPN/notify endpoint – server-to-server)
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> MoMoIPN()
        {
            _logger.LogInformation("MoMo IPN received");
            var response = await _moMoService.PaymentExecute(Request.Query);

            if (!response.Success)
            {
                _logger.LogWarning("MoMo IPN: signature invalid or payment failed");
                return Json(new { message = "Lỗi xác thực" });
            }

            var momoOrderId = Request.Query["orderId"].ToString();
            int orderId = 0;
            var parts = momoOrderId.Split('-');
            if (parts.Length >= 2) int.TryParse(parts[1], out orderId);

            if (orderId > 0)
            {
                var order = _orderService.GetOrderById(orderId);
                if (order != null && order.Status == "Pending")
                {
                    _orderService.UpdateOrderStatus(orderId, "Processing");
                    _paymentService.UpdatePaymentStatus(orderId, "Success", response.TransactionId);
                    _logger.LogInformation("MoMo IPN: order {OrderId} updated to Processing", orderId);
                }
            }

            return Json(new { message = "OK" });
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

