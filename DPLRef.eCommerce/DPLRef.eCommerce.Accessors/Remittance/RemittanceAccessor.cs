using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Accessors.Sales;
using System.Linq;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Accessors.Remittance
{
    class RemittanceAccessor : AccessorBase, IRemittanceAccessor
    {
        public SellerOrderData[] SalesTotal()
        {
            var result = new List<SellerOrderData>();

            //using (var conn = new SqlConnection(DatabaseConnectionString))
            //{
            //    conn.Open();

            //    using (var cmd = new SqlCommand("BackendSalesTotals", conn))
            //    {
            //        cmd.CommandType = System.Data.CommandType.StoredProcedure;

            //        using (var reader = cmd.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                var item = new SellerOrderData()
            //                {
            //                    SellerId = (int)reader["SellerId"],
            //                    SellerName = (string)reader["SellerName"],
            //                    OrderCount = (int)reader["OrderCount"],
            //                    OrderTotal = Convert.ToDecimal(reader["OrderTotal"])
            //                };
            //                result.Add(item);
            //            }
            //        }
            //    }
            //}
            using (var db = eCommerce.Accessors.EntityFramework.eCommerceDbContext.Create())
            {
                var backendSalesTotalQuery = (from o in db.Orders
                                             join s in db.Sellers on o.SellerId equals s.Id
                                             group o by new { o.SellerId, s.Name } into g
                                             select new SellerOrderData
                                             {
                                                 SellerId = g.Key.SellerId,
                                                 SellerName = g.Key.Name,
                                                 OrderCount = g.Count(),
                                                 OrderTotal = g.Sum(x => x.Total)
                                             }).ToArray();

                foreach (var line in backendSalesTotalQuery)
                {
                    result.Add(line);
                }
            }

            return result.ToArray();
        }
    }
}
