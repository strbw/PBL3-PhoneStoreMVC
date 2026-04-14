using HDKmall.Models;

namespace HDKmall.ViewModels
{
    public class PaymentVM
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderCode { get; set; }
        public string ReturnUrl { get; set; }
        public string IpnUrl { get; set; }
    }

    public class VNPaymentResponseVM
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int OrderId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public class PaymentHistoryVM
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
