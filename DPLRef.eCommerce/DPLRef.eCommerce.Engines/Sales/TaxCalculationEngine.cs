using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Contracts.WebStore.Sales;
using System;

namespace DPLRef.eCommerce.Engines.Sales
{
    class TaxCalculationEngine : EngineBase, ITaxCalculationEngine
    {
        public override string TestMe(string input)
        {
            input = base.TestMe(input);
            return input;
        }

        public WebStoreCart CalculateCartTax(WebStoreCart cart)
        {
            // TODO: Make this real. Mocking for now

            foreach (var item in cart.CartItems)
            {
                cart.TaxAmount += Math.Round(item.ExtendedPrice * 0.07m, 2); // just assuming 7% tax for everything for now
            }

            // update the cart total with the tax amount
            cart.Total += Math.Round(cart.TaxAmount, 2);

            return cart;
        }
    }
}
