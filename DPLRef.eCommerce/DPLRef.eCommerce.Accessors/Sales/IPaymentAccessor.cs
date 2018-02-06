using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Accessors.Sales
{
    public interface IPaymentAccessor : IServiceContractBase
    {
        PaymentAuthStatusResult Authorize(PaymentInstrument paymentInstrument, int orderId, decimal total);

        PaymentCaptureResult Capture(string authorizationCode);
    }
}
