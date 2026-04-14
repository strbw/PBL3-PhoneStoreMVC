using HDKmall.BLL.Interfaces;
using HDKmall.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace HDKmall.BLL.Services
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;
        private readonly string _vnPayUrl;
        private readonly string _tmnCode;
        private readonly string _hashSecret;

        public VNPayService(IConfiguration configuration)
        {
            _configuration = configuration;
            _vnPayUrl = _configuration["VNPay:Url"] ?? "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            _tmnCode = _configuration["VNPay:TmnCode"] ?? "TESTMERCHANT";
            _hashSecret = _configuration["VNPay:HashSecret"] ?? "TESTSECRET";
        }

        public string CreatePaymentUrl(PaymentVM model, HttpContext context)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var requestId = Random.Shared.Next(100000).ToString();

            var vnpayData = new SortedDictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", _tmnCode },
                { "vnp_Amount", ((long)model.TotalAmount * 100).ToString() },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", GetIpAddress(context) },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", $"Thanh toan HD {model.OrderId}" },
                { "vnp_OrderType", "order" },
                { "vnp_ReturnUrl", model.ReturnUrl },
                { "vnp_TxnRef", model.OrderCode },
                { "vnp_ExpireDate", DateTime.Now.AddHours(1).ToString("yyyyMMddHHmmss") }
            };

            var queryBuilder = new StringBuilder();
            foreach (var item in vnpayData)
            {
                queryBuilder.AppendFormat("&{0}={1}", Uri.EscapeDataString(item.Key), Uri.EscapeDataString(item.Value));
            }

            var queryString = queryBuilder.ToString();
            var secureHash = HmacSHA512(_hashSecret, queryString.Substring(1));
            var paymentUrl = $"{_vnPayUrl}?{queryString}&vnp_SecureHash={secureHash}";

            return paymentUrl;
        }

        public VNPaymentResponseVM PaymentExecute(IQueryCollection collections)
        {
            var vnpayData = new SortedDictionary<string, string>();
            var responseCode = "";
            var orderId = "";
            var amount = 0L;
            var transactionId = "";
            var transactionNo = "";
            var responseDescription = "";

            foreach (var key in collections.Keys)
            {
                if (key.StartsWith("vnp_"))
                {
                    vnpayData.Add(key, collections[key].ToString());
                }
            }

            if (vnpayData.ContainsKey("vnp_ResponseCode"))
                responseCode = vnpayData["vnp_ResponseCode"];
            if (vnpayData.ContainsKey("vnp_TxnRef"))
                orderId = vnpayData["vnp_TxnRef"];
            if (vnpayData.ContainsKey("vnp_Amount"))
                long.TryParse(vnpayData["vnp_Amount"], out amount);
            if (vnpayData.ContainsKey("vnp_TransactionNo"))
                transactionNo = vnpayData["vnp_TransactionNo"];
            if (vnpayData.ContainsKey("vnp_TransactionStatus"))
                transactionId = vnpayData["vnp_TransactionStatus"];
            if (vnpayData.ContainsKey("vnp_SecureHash"))
                vnpayData.Remove("vnp_SecureHash");
            if (vnpayData.ContainsKey("vnp_SecureHashType"))
                vnpayData.Remove("vnp_SecureHashType");

            var queryString = "";
            foreach (var item in vnpayData)
            {
                queryString += "&" + Uri.EscapeDataString(item.Key) + "=" + Uri.EscapeDataString(item.Value);
            }

            var secureHash = HmacSHA512(_hashSecret, queryString.Substring(1));
            var vnp_SecureHash = collections["vnp_SecureHash"].ToString();

            var isValid = secureHash == vnp_SecureHash && responseCode == "00";

            return new VNPaymentResponseVM
            {
                Success = isValid,
                Message = isValid ? "Thanh toán thành công" : "Thanh toán thất bại",
                OrderId = int.TryParse(orderId, out var id) ? id : 0,
                TransactionId = transactionNo,
                Amount = amount / 100m,
                TransactionDate = DateTime.Now
            };
        }

        private string GetIpAddress(HttpContext context)
        {
            var ipAddress = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ??
                           context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                           context.Connection.RemoteIpAddress?.ToString();

            return ipAddress ?? "127.0.0.1";
        }

        private string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }
    }
}
