using HDKmall.ViewModels;

namespace HDKmall.BLL.Interfaces
{
    public interface IPaymentService
    {
        void CreatePayment(int orderId, string paymentMethod, decimal amount);
        PaymentHistoryVM GetPaymentHistory(int orderId);
        void UpdatePaymentStatus(int orderId, string status, string transactionId);
    }
}
