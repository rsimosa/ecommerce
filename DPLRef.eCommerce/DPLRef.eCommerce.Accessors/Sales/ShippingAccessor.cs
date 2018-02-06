using System;
using DPLRef.eCommerce.Accessors.DataTransferObjects;

namespace DPLRef.eCommerce.Accessors.Sales
{
    class ShippingAccessor : AccessorBase, IShippingAccessor
    {
        public ShippingResult RequestShipping(int orderId)
        {
            // if orderId <= 0, lets fail.
            if (orderId <= 0)
                return new ShippingResult()
                {
                    Success = false,
                    ErrorCode = 1,
                    ErrorMessage = "Unit Test Mock Error"
                };

            // else return success
            return new ShippingResult()
            {
                Success = true,
                ShippingProvider = "UPS",
                TrackingCode = Guid.NewGuid().ToString() // use guid for tracking code 
            };
        }
    }
}
