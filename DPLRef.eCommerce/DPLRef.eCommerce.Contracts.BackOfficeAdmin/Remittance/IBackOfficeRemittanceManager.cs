using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.BackOfficeAdmin.Remittance
{
    public interface IBackOfficeRemittanceManager : IServiceContractBase
    {
        OrderDataResponse Totals();
    }
}
