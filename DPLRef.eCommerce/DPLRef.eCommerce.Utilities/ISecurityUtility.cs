using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Utilities
{
    public interface ISecurityUtility : IServiceContractBase
    {
        bool SellerAuthenticated();

        bool BackOfficeAdminAuthenticated();
    }
}
