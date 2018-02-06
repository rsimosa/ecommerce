using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Engines.Notification
{
    public interface IEmailFormattingEngine : IServiceContractBase
    {
        string FormatOrderEmailBody(Order order, Seller seller);
    }
}
