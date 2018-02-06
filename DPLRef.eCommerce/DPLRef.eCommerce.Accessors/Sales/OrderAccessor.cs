using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Accessors.Sales
{
    class OrderAccessor : AccessorBase, IOrderAccessor
    {
        public Order SaveOrder(int catalogId, Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");
            if (order.OrderLines == null || order.OrderLines.Length == 0)
                throw new ArgumentException("OrderLines must have at least 1 item.");
            if (order.BillingAddress == null)
                throw new ArgumentException("BillingAddress must not be  null.");

            EntityFramework.Order model = null;

            using (var db = new EntityFramework.eCommerceDbContext())
            {
                var isNewOrder = false;

                if (order.Id > 0)
                {
                    model = db.Orders.Find(order.Id);
                }
                else
                {
                    isNewOrder = true;
                    model = new EntityFramework.Order();
                    db.Orders.Add(model);
                }

                DTOMapper.MapOrder(order, model);
                DTOMapper.MapBilling(order.BillingAddress, model);
                if (order.ShippingAddress != null)
                    DTOMapper.MapShipping(order.ShippingAddress, model);
                else
                    DTOMapper.MapShipping(new Address(), model); // copy a blank address

                model.SellerId = Context.SellerId;
                model.CatalogId = catalogId;
                db.SaveChanges();

                // NOTE: For now, We are intentionally not allowing edits/adds of order lines for existing orders
                if (isNewOrder)
                {
                    foreach (var line in order.OrderLines)
                    {
                        var orderLineModel = new EntityFramework.OrderLine()
                        {
                            OrderId = model.Id,
                            ExtendedPrice = line.ExtendedPrice,
                            ProductId = line.ProductId,
                            Quantity = line.Quantity,
                            UnitPrice = line.UnitPrice,
                            ProductName = line.ProductName
                        };
                        db.OrderLines.Add(orderLineModel);

                        db.SaveChanges();
                    }
                }
            }

            return FindOrder(model.Id);
        }

        public Order FindOrder(int id)
        {
            Order order = null;
            if (id > 0)
            {
                using (var db = new EntityFramework.eCommerceDbContext())
                {
                    EntityFramework.Order model = db.Orders.Find(id);
                    if (model != null)
                    {
                        order = DTOMapper.MapOrder(model);

                        var orderItemModels = from ol in db.OrderLines where ol.OrderId == id select ol;
                        var orderLines = new List<OrderLine>();

                        foreach (var cim in orderItemModels)
                        {
                            var orderLine = DTOMapper.Map<OrderLine>(cim);
                            orderLines.Add(orderLine);
                        }

                        order.OrderLines = orderLines.ToArray();
                    }
                }
            }
            return order;
        }

        public SellerSalesTotal SalesTotal()
        {
            SellerSalesTotal result = null;

            using (var conn = new SqlConnection(DatabaseConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("SalesTotalForSeller", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("SellerId", Context.SellerId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new SellerSalesTotal()
                            {
                                SellerId = (int)reader["SellerId"],
                                SellerName = (string)reader["SellerName"],
                                OrderCount = (int)reader["OrderCount"],
                                OrderTotal = Convert.ToDecimal(reader["OrderTotal"])
                            };
                        }
                    }
                }
            }

            return result;
        }

        public Order[] UnfulfilledOrders()
        {
            var orderList = new List<Order>();
            int[] orderIds;
            using (var db = new EntityFramework.eCommerceDbContext())
            {
                var list = from o in db.Orders where o.SellerId == Context.SellerId && o.Status == OrderStatuses.Authorized select o.Id;
                orderIds = list.ToArray();
            }

            foreach (var id in orderIds)
            {
                orderList.Add(FindOrder(id));
            }

            return orderList.ToArray();
        }

        public Order UpdateOrderStatus(int orderId, OrderStatuses status, string notes)
        {
            if (orderId < 1) // order should never be new
                throw new ArgumentException("Cannot pass new order");

            using (var db = new EntityFramework.eCommerceDbContext())
            {
                var model = db.Orders.Find(orderId);

                // update the fields that are related to status changes
                if (model != null)
                {
                    model.Status = status;
                    model.Notes = notes;

                    db.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Order not found");
                }
            }
            return FindOrder(orderId);
        }

        public Order FulfillOrder(int orderId, string shippingProvider, string trackingCode, string notes)
        {
            if (orderId < 1) // order should never be new
                throw new ArgumentException("Cannot pass new order");

            using (var db = new EntityFramework.eCommerceDbContext())
            {
                var model = db.Orders.Find(orderId);

                // update the fields that are related to fulfillment
                if (model != null)
                {
                    model.Status = OrderStatuses.Shipped;
                    model.ShippingProvider = shippingProvider;
                    model.TrackingCode = trackingCode;
                    model.Notes = notes;

                    db.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Order not found");
                }
            }
            return FindOrder(orderId);
        }
    }
}
