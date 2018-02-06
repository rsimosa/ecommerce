using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Common.Contracts;
using System;
using System.Linq;

namespace DPLRef.eCommerce.Tests.Mocks
{
    public class MockOrderAccessor : MockBase, IOrderAccessor
    {
        public MockOrderAccessor(MockData data) : base(data)
        {

        }

        public Order FindOrder(int id)
        {
            if (MockData.ForceException)
                throw new ArgumentException();

            return this.MockData.Orders.FirstOrDefault(o => o.Id == id);
        }

        public Order FulfillOrder(int orderId, string shippingProvider, string trackingCode, string notes)
        {
            var order = this.MockData.Orders.FirstOrDefault(o => o.Id == orderId);
            order.Status = OrderStatuses.Shipped;
            order.ShippingProvider = shippingProvider;
            order.TrackingCode = trackingCode;
            MockData.OrderCaptureAttempted = true;
            MockData.OrderFulfilled = true;
            return order;
        }

        public SellerSalesTotal SalesTotal()
        {
            return MockData.SellerSalesTotal;
        }

        public Order SaveOrder(int catalogId, Order order)
        {
            if (order.Id == 0)
            {
                MockData.OrderCreated = true;
                if (MockData.Orders.Count > 0)
                    order.Id = MockData.Orders.Max(x => x.Id) + 1;
                else
                    order.Id = 1;
            }            

            var found = MockData.Orders.FirstOrDefault(x => x.Id == order.Id);
            if (found != null)
            {
                MockData.Orders.Remove(found);                
            }
            MockData.Orders.Add(order);

            return order;
        }

        public string TestMe(string input)
        {
            return input;
        }

        public Order[] UnfulfilledOrders()
        {
            var orders = this.MockData.Orders.Where(o => o.Status == OrderStatuses.Authorized).ToArray();
            return orders;
        }

        public Order UpdateOrderStatus(int orderId, OrderStatuses status, string notes)
        {
            var order = this.MockData.Orders.FirstOrDefault(o => o.Id == orderId);
            order.Status = status;
            order.Notes = notes;
            if (status == OrderStatuses.Captured)
            {
                MockData.OrderCapturedStatus = true;
            }
            return order;
        }
    }
}
