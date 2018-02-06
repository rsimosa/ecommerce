using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Accessors.Sales;

namespace DPLRef.eCommerce.Engines.Sales
{
    public interface IOrderValidationEngine
    {
        ValidationResponse ValidateOrder(Order order);
    }
}
