using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Accessors.DataTransferObjects;

namespace DPLRef.eCommerce.Accessors.Remittance
{
    public interface ISellerAccessor : IServiceContractBase
    {
        Seller Find(int id);

        Seller Save(Seller seller);

        void Delete(int id);
    }
}
