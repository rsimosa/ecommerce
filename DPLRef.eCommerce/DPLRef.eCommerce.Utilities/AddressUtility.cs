using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Utilities
{
    class AddressUtility : IAddressUtility
    {
        public bool ValidateAddress(Address address)
        {
            if (address == null)
            {
                return false;
            }

            // NOTE: we don't care if Addr2 is null or empty
            if (string.IsNullOrEmpty(address.Addr1)
                || string.IsNullOrEmpty(address.City)
                || string.IsNullOrEmpty(address.Postal)
                || string.IsNullOrEmpty(address.State)
                || string.IsNullOrEmpty(address.First)
                || string.IsNullOrEmpty(address.Last))
            {
                return false;
            }

            // all is well
            return true;
        }
    }
}
