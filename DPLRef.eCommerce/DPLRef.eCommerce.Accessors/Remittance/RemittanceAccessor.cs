using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Accessors.Sales;

namespace DPLRef.eCommerce.Accessors.Remittance
{
    class RemittanceAccessor : AccessorBase, IRemittanceAccessor
    {
        public SellerOrderData[] SalesTotal()
        {
            var result = new List<SellerOrderData>();

            using (var conn = new SqlConnection(DatabaseConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("BackendSalesTotals", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new SellerOrderData()
                            {
                                SellerId = (int)reader["SellerId"],
                                SellerName = (string)reader["SellerName"],
                                OrderCount = (int)reader["OrderCount"],
                                OrderTotal = Convert.ToDecimal(reader["OrderTotal"])
                            };
                            result.Add(item);
                        }
                    }
                }
            }

            return result.ToArray();
        }
    }
}
