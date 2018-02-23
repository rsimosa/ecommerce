using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using System;
using System.Data.SqlClient;
using System.Transactions;

namespace DPLRef.eCommerce.Accessors.EntityFramework
{
    public static class DbContextFactory
    {
        public static void CreateGlobalContext()
        {
            UnitTestContext = new eCommerceDbContext()
            {
                AllowDispose = false
            };
            UnitTestContext.Database.BeginTransaction();
        }

        public static void CancelGlobalTransaction()
        {
            if (UnitTestContext != null)
            {
                UnitTestContext.Database.RollbackTransaction();
                UnitTestContext.AllowDispose = true;
                UnitTestContext.Dispose();
                UnitTestContext = null;
            }
        }

        public static void CommitGlobalTransaction()
        {
            if (UnitTestContext != null)
            {
                UnitTestContext.Database.CommitTransaction();
            }
        }

        internal static eCommerceDbContext UnitTestContext { get; private set; }

        internal static eCommerceDbContext Create()
        {
            if (DbContextFactory.UnitTestContext == null)
                return new eCommerceDbContext()
                {
                    AllowDispose = true
                };
            return UnitTestContext;
        }
    }

    internal class eCommerceDbContext : DbContext
    {
        public eCommerceDbContext()
            : base()

        {

        }

        protected IConfigurationRoot Configuration { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();
            Configuration = builder.Build();



            string connectionString = Configuration["eCommerceDatabase"];
            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
            else
            {
                connectionString = Configuration["eCommerceDatabaseSqlite"];

                if (string.IsNullOrEmpty(connectionString))
                    throw new InvalidOperationException("Connection string environment variable missing.");

                optionsBuilder.UseSqlite(connectionString);
            }
        }

        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Catalog> Catalogs { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<OrderLine> OrderLines { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        public bool AllowDispose { get; set; } = true;

        public override void Dispose()
        {
            // this is the secret of the wrapper, without this do nothing we won't handle rolling back transactions
            // only dispose if we are allowing it to dispose
            if (AllowDispose)
                base.Dispose();
        }

    }
}
