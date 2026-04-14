using HDKmall.BLL.Interfaces;
using HDKmall.ViewModels;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace HDKmall.BLL.Services
{
    public class MoMoService : IMoMoService
    {
        private readonly IConfiguration _configuration;
        private readonly string _momoUrl;
        private readonly string _partnerCode;
        private readonly string _accessKey;
        private readonly string _secretKey;

        public MoMoService(IConfiguration configuration)
        {
            _configuration = configuration;
            _momoUrl = _configuration["MoMo:Url"] ?? "https://test-payment.momo.vn/v3/gateway/api/create";
            _partnerCode = _configuration["MoMo:PartnerCode"] ?? "MOMERCHANT";
            _accessKey = _configuration["MoMo:AccessKey"] ?? "F8591820140105";
            _secretKey = _configuration["MoMo:SecretKey"] ?? "bJisuQtDXiVD7VQ3Dij05sDHxGT205fW";
        }

        public async Task<string> CreatePaymentUrl(PaymentVM model, HttpContext context)
        {
            var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var requestId = Guid.NewGuid().ToString();

            var rawData = $"accessKey={_accessKey}&amount={((long)model.TotalAmount * 1000)}" +
                         $"&extraData=&ipAddress={GetIpAddress(context)}&lang=vi" +
                         $"&orderId={model.OrderCode}&orderInfo=Thanh+toan+don+hang+{model.OrderId}" +
                         $"&partnerCode={_partnerCode}&redirectUrl={model.ReturnUrl}" +
                         $"&requestId={requestId}&requestType=captureWallet&timestamp={timeStamp}";

            var signature = ComputeHmacSHA256(rawData, _secretKey);

            var momoRequest = new
            {
                accessKey = _accessKey,
                partnerCode = _partnerCode,
                requestId = requestId,
                amount = ((long)model.TotalAmount * 1000).ToString(),
                orderId = model.OrderCode,
                orderInfo = $"Thanh toan don hang {model.OrderId}",
                redirectUrl = model.ReturnUrl,
                ipAddress = GetIpAddress(context),
                requestType = "captureWallet",
                extraData = "",
                lang = "vi",
                timestamp = timeStamp,
                signature = signature
            };

            using (var client = new HttpClient())
            {
                try
                {
                    var json = JsonSerializer.Serialize(momoRequest);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(_momoUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var jsonDoc = JsonDocument.Parse(responseContent);
                        var root = jsonDoc.RootElement;

                        if (root.TryGetProperty("payUrl", out var payUrl))
                        {
                            return payUrl.GetString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi kết nối MoMo: {ex.Message}");
                }
            }

            return "";
        }

        public async Task<VNPaymentResponseVM> PaymentExecute(IQueryCollection collections)
        {
            var orderId = collections["orderId"].ToString();
            var message = collections["message"].ToString();
            var responseCode = collections["resultCode"].ToString();
            var requestId = collections["requestId"].ToString();
            var amount = collections["amount"].ToString();

            var isSuccess = responseCode == "0";

            return new VNPaymentResponseVM
            {
                Success = isSuccess,
                Message = isSuccess ? "Thanh toán thành công" : $"Thanh toán thất bại: {message}",
                OrderId = int.TryParse(orderId, out var id) ? id : 0,
                TransactionId = requestId,
                Amount = string.IsNullOrEmpty(amount) ? 0 : decimal.Parse(amount) / 1000,
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

        private string ComputeHmacSHA256(string message, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
            }
        }
    }
}
