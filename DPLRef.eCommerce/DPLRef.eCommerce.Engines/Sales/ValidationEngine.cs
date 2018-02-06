using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Utilities;

namespace DPLRef.eCommerce.Engines.Sales
{
    class ValidationEngine : EngineBase, IOrderValidationEngine
    {
        #region IOrderValidation

        public ValidationResponse ValidateOrder(Order order)
        {
            var response = new ValidationResponse()
                {Success = false};
            decimal calculatedSubtotal = 0;

            // Make sure we don't have a null input
            if (order == null)
            {
                response.Message = "Order is invalid";
                return response;
            }

            // Make sure the cart has items for purchase
            if (order.OrderLines == null || order.OrderLines.Length == 0)
            {
                response.Message = "Order has no order lines";
                return response;
            }

            // make sure the cart amounts have been determined
            foreach (var orderLine in order.OrderLines)
            {
                decimal lineTotal = (orderLine.UnitPrice * orderLine.Quantity);
                if (lineTotal != orderLine.ExtendedPrice)
                {
                    response.Message = "Invalid order line pricing";
                    return response;
                }

                calculatedSubtotal += lineTotal;
            }

            // make sure line amounts sum to order amounts
            if (order.SubTotal != calculatedSubtotal)
            {
                response.Message = "Invalid order subtotal";
                return response;
            }

            // make sure remaining totals are valid
            if ((order.SubTotal + order.TaxAmount) != order.Total)
            {
                response.Message = "Invalid order total";
                return response;
            }

            // make sure the shipping address is valid and complete
            var addressUtility = UtilityFactory.CreateUtility<IAddressUtility>();
            if (!addressUtility.ValidateAddress(order.ShippingAddress))
            {
                response.Message = "ShippingAddress address is not valid";
                return response;
            }

            // if there is a cart total, make sure the billign address is valid and complete
            if (order.SubTotal > 0 && !addressUtility.ValidateAddress(order.BillingAddress))
            {
                response.Message = "BillingAddress address is not valid";
                return response;
            }

            // all is good
            return new ValidationResponse()
            {
                Success = true
            };
        }        

        #endregion

    }
}
