using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.Admin.Sales
{
    public interface IAdminRemittanceManager : IServiceContractBase
    {
        SalesTotalsResponse Totals();
    }
}
