using Microsoft.Data.Sqlite;
using System;
using System.Data.SqlClient;

namespace DPLRef.eCommerce.Database
{
    static class SqliteSeedData
    {
        public static void Add(string connectionString)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                var sellerId = CreateSeller(conn); // Seller ID == 1 -- used in test clients
                var catalogid = CreateCatalog(conn, sellerId, "TEST_CATALOG");
                CreateProduct(conn, catalogid, true, true); // product that IS available and IS downloadable
                CreateProduct(conn, catalogid, true, true); // product that IS available and IS downloadable
                catalogid = CreateCatalog(conn, sellerId, "TEST_CATALOG_2");
                CreateProduct(conn, catalogid, false, false); // product that is NOT available OR downloadable

                CreateSeller(conn); // Seller ID == 2 -- used in test clients
                //  CreateSeller(conn); // Seller ID == 3 -- used for no sales tests -- do not use in test clients
            }
        }

        static int CreateSeller(SqliteConnection connection)
        {
            var sqlite = @"
                insert into Sellers
                    (Name, UserName)
                values
                    (@name, 'bsmith');
        
                select last_insert_rowid();
            ";

            using (var cmd = new SqliteCommand(sqlite, connection))
            {
                cmd.Parameters.AddWithValue("name", "TEST_SELLER");
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    var id = Convert.ToInt32(reader[0]);
                    return id;
                }
            }
        }

        static int CreateCatalog(SqliteConnection connection, int sellerId, string catalogName)
        {
            var sqlite = @"
                insert into Catalogs
                    (Name, SellerId, Description)
                values
                    (@name, @sellerid, @description); 

                select last_insert_rowid();
            ";

            using (var cmd = new SqliteCommand(sqlite, connection))
            {
                cmd.Parameters.AddWithValue("name", catalogName);
                cmd.Parameters.AddWithValue("sellerid", sellerId);
                cmd.Parameters.AddWithValue("description", $"{catalogName} description");
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    var id = Convert.ToInt32(reader[0]);
                    return id;
                }
            }
        }

        static int CreateProduct(SqliteConnection connection, int catalogId, bool isAvailable, bool IsDownloadable)
        {
            var sqlite = @"
                insert into Products
                    (Name, CatalogId, IsDownloadable, Price, Shippingweight, IsAvailable, Summary, Detail, SupplierName)
                values
                    (@name, @catalogid, @isdownloadable, 5, 1.0, @isavailable, 'TEST_PRODUCT summary', 'TEST_PRODUCT detail', 'TEST_PRODUCT supplier name'); 

                select last_insert_rowid();
            ";

            using (var cmd = new SqliteCommand(sqlite, connection))
            {
                cmd.Parameters.AddWithValue("name", "TEST_PRODUCT");
                cmd.Parameters.AddWithValue("catalogid", catalogId);
                cmd.Parameters.AddWithValue("isavailable", isAvailable);
                cmd.Parameters.AddWithValue("isdownloadable", IsDownloadable);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    var id = Convert.ToInt32(reader[0]);
                    return id;
                }
            }
        }
    }
}
