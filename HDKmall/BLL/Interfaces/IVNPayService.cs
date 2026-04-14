using HDKmall.ViewModels;

namespace HDKmall.BLL.Interfaces
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(PaymentVM model, HttpContext context);
        VNPaymentResponseVM PaymentExecute(IQueryCollection collections);
    }
}
