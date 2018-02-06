using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Utilities
{
    public interface IAddressUtility
    {
        bool ValidateAddress(Address address);
    }
}
