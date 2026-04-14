using HDKmall.ViewModels;

namespace HDKmall.BLL.Interfaces
{
    public interface IMoMoService
    {
        Task<string> CreatePaymentUrl(PaymentVM model, HttpContext context);
        Task<VNPaymentResponseVM> PaymentExecute(IQueryCollection collections);
    }
}
