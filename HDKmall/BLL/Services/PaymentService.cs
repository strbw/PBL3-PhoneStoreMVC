using HDKmall.BLL.Interfaces;
using HDKmall.ViewModels;

namespace HDKmall.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private static Dictionary<int, PaymentHistoryVM> _paymentHistory = new();

        public void CreatePayment(int orderId, string paymentMethod, decimal amount)
        {
            var payment = new PaymentHistoryVM
            {
                OrderId = orderId,
                PaymentMethod = paymentMethod,
                Amount = amount,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            if (!_paymentHistory.ContainsKey(orderId))
            {
                _paymentHistory[orderId] = payment;
            }
        }

        public PaymentHistoryVM GetPaymentHistory(int orderId)
        {
            return _paymentHistory.ContainsKey(orderId) ? _paymentHistory[orderId] : null;
        }

        public void UpdatePaymentStatus(int orderId, string status, string transactionId)
        {
            if (_paymentHistory.ContainsKey(orderId))
            {
                _paymentHistory[orderId].Status = status;
                _paymentHistory[orderId].TransactionId = transactionId;
            }
        }
    }
}
