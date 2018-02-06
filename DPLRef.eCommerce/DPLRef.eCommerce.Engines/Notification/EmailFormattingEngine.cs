using System.Text;
using DPLRef.eCommerce.Accessors.DataTransferObjects;

namespace DPLRef.eCommerce.Engines.Notification
{
    class EmailFormattingEngine : EngineBase, IEmailFormattingEngine
    {
        public string FormatOrderEmailBody(Order order, Seller seller)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Order Notification Email");
            sb.AppendLine("==============================");
            sb.AppendLine($"Order ID: {order.Id}");
            sb.AppendLine($"Webstore: {seller.Name}");
            sb.AppendLine($"Status: {order.Status}");
            sb.AppendLine($"Shipping Provider: {order.ShippingProvider}");
            sb.AppendLine($"Tracking Code: {order.TrackingCode}");
            sb.AppendLine("==============================");            
            foreach (var orderLine in order.OrderLines)
            {
                sb.AppendLine(
                    $"Product: {orderLine.ProductName}, Quantity: {orderLine.Quantity}, Unit Price: ${orderLine.UnitPrice}, Extended Price: ${orderLine.ExtendedPrice}");
            }
            sb.AppendLine("==============================");
            sb.AppendLine($"Subtotal: ${order.SubTotal}");
            sb.AppendLine($"Tax: ${order.TaxAmount}");
            sb.AppendLine($"Total: ${order.Total}");

            return sb.ToString();
        }
    }
}
