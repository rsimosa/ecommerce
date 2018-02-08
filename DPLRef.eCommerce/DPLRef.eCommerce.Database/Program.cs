using System;
using System.Reflection;
using System.Data.SqlClient;
using DbUp;
using DbUp.Engine;

namespace DPLRef.eCommerce.Database
{
    class Program
    {
        static void CleanupTables(string connectionString)
        {
            var sql = @"
                if exists (select * from INFORMATION_SCHEMA.TABLES  t where t.TABLE_NAME='SchemaVersions')
                    drop table SchemaVersions;

                if exists (select * from INFORMATION_SCHEMA.TABLES  t where t.TABLE_NAME='OrderLines')
                    drop table OrderLines;

                if exists (select * from INFORMATION_SCHEMA.TABLES  t where t.TABLE_NAME='Orders')
                    drop table Orders;

                if exists (select * from INFORMATION_SCHEMA.TABLES  t where t.TABLE_NAME='CartItems')
                    drop table CartItems;

                if exists (select * from INFORMATION_SCHEMA.TABLES  t where t.TABLE_NAME='Carts')
                    drop table Carts;

                if exists (select * from INFORMATION_SCHEMA.TABLES  t where t.TABLE_NAME='Products')
                    drop table Products;

                if exists (select * from INFORMATION_SCHEMA.TABLES  t where t.TABLE_NAME='Catalogs')
                    drop table Catalogs;

                if exists (select * from INFORMATION_SCHEMA.TABLES  t where t.TABLE_NAME='Sellers')
                    drop table Sellers;

                if exists(select * from INFORMATION_SCHEMA.ROUTINES t where t.ROUTINE_NAME = 'SalesTotalForSeller')
                    drop procedure SalesTotalForSeller;

                if exists(select * from INFORMATION_SCHEMA.ROUTINES t where t.ROUTINE_NAME = 'BackendSalesTotals')
                    drop procedure BackendSalesTotals;
            ";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static int Main(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("eCommerceDatabase");

            EnsureDatabase.For.SqlDatabase(connectionString);

#if DEBUG
            CleanupTables(connectionString);
#endif

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

#if DEBUG
            SeedData.Add(connectionString);
#endif

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();

            Console.ReadLine();

            return 0;
        }
    }
}
