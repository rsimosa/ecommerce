﻿using System;
using System.Reflection;
using System.Data.SqlClient;
using DbUp;
using DbUp.Engine;
using DbUp.SQLite;
using System.IO;
using Microsoft.Data.Sqlite;
using DPLRef.eCommerce.Common.Shared;

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

        static void CleanupTablesSqlite(string filepath)
        {
            File.Delete(filepath);
        }

        static int Main(string[] args)
        {
            var sqlServer = true;
            var connectionString = Config.SqlServerConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                // Example value for eCommerceDatabaseSqlite: 
                // Data Source=C:\Workspaces\eCommerce-Core\DPLRef.eCommerce\eCommerce.sqlite
                connectionString = Config.SqliteConnectionString;
                sqlServer = false;

                if (string.IsNullOrEmpty(connectionString))
                    throw new InvalidOperationException("Connection string environment variable missing.");
            }


            DatabaseUpgradeResult result = null;

            if (sqlServer)
            {
                EnsureDatabase.For.SqlDatabase(connectionString);

                CleanupTables(connectionString);

                var upgrader =
                    DeployChanges.To
                        .SqlDatabase(connectionString)
                        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), (scriptname) =>
                        {
                            if (scriptname.StartsWith("Database.Scripts"))
                                return true;
                            return false;
                        })
                        .LogToConsole()
                        .Build();

                result = upgrader.PerformUpgrade();
            }
            else
            {
                var filePath = connectionString.Split("Data Source=")[1];
                CleanupTablesSqlite(filePath);

                var upgrader =
                     DeployChanges.To
                         .SQLiteDatabase(connectionString)
                         .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), (scriptname) =>
                         {
                             if (scriptname.StartsWith("Database.SqliteScripts"))
                                 return true;
                             return false;
                         })
                         .LogToConsole()
                         .Build();

                result = upgrader.PerformUpgrade();
            }

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

            if (sqlServer)
            {
                SeedData.Add(connectionString);

            }
            else
            {
                SqliteSeedData.Add(connectionString);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();

            Console.ReadLine();

            return 0;
        }
    }
}
