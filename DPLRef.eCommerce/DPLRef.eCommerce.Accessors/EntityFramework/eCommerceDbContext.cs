using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using System;
using System.Data.SqlClient;
using System.Transactions;


namespace DPLRef.eCommerce.Accessors.EntityFramework
{
    internal class eCommerceDbContext : DbContext
    {
        internal static eCommerceDbContext UnitTestContext { get; set; }

        // Everyone that uses the eCommerceDbContext will use this 
        // constructor method
        internal static eCommerceDbContext Create(bool allowDispose = true)
        {
            if (UnitTestContext == null)
                return new eCommerceDbContext()
                {
                    AllowDispose = allowDispose
                };
            return UnitTestContext;
        }

        private eCommerceDbContext()
            : base()

        {

        }

        public override void Dispose()
        {
            // this is the secret of the wrapper, without this do nothing we won't handle rolling back transactions
            // only dispose if we are allowing it to dispose
            if (AllowDispose)
                base.Dispose();
        }

        protected IConfigurationRoot Configuration { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
.AddEnvironmentVariables();
            Configuration = builder.Build();

            base.OnConfiguring(optionsBuilder);

            string connectionString = Common.Shared.Config.SqlServerConnectionString;
            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
            else
            {
                connectionString = Common.Shared.Config.SqliteConnectionString;

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

    }
}
