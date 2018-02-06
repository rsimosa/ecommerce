using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Accessors.Catalog;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.IO;
using Microsoft.Extensions.Configuration.Memory;
using DPLRef.eCommerce.Accessors.EntityFramework;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Accessors.Remittance;

namespace DPLRef.eCommerce.Tests.AccessorTests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [TestClass]
    public abstract class DbTestAccessorBase
    {
        public IConfigurationRoot Configuration { get; set; }

        public DbTestAccessorBase()
        {
            var source = new MemoryConfigurationSource();
            source.InitialData = 
                new List<KeyValuePair<string, string>>
                    { };

            var builder = new ConfigurationBuilder();
            builder.Add(source);
            Configuration = builder.Build();
        }
        
        [TestInitialize()]
        public void Init()
        {
            var builder = new ConfigurationBuilder()                 
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var db = Configuration["eCommerceDatabase"];

            DbContextFactory.CreateGlobalContext();            
        }

        [TestCleanup()]
        public void Cleanup()
        {
            DbContextFactory.CancelGlobalTransaction();
        }


        private AmbientContext _context = new AmbientContext()
        {
            SellerId = 1
        };
        protected AmbientContext Context
        {
            get
            {
                return _context;
            }
        }

        protected ICartAccessor CreateCartAccessor(Guid sessionId)
        {
            _context.SessionId = sessionId;
            var accessor = new AccessorFactory(_context, new UtilityFactory(_context));
            var result = accessor.CreateAccessor<ICartAccessor>();
            return result;
        }

        protected ICatalogAccessor CreateCatalogAccessor(int sellerId = 1)
        {
            _context.SellerId = sellerId;
            var accessor = new AccessorFactory(_context, new UtilityFactory(_context));
            var result = accessor.CreateAccessor<ICatalogAccessor>();
            return result;
        }

        protected IOrderAccessor CreateOrderAccessor(int sellerId = 1)
        {
            _context.SellerId = sellerId;
            var accessor = new AccessorFactory(_context, new UtilityFactory(_context));
            var result = accessor.CreateAccessor<IOrderAccessor>();
            return result;
        }

        protected IRemittanceAccessor CreateRemittanceAccessor()
        {
            var accessor = new AccessorFactory(_context, new UtilityFactory(_context));
            var result = accessor.CreateAccessor<IRemittanceAccessor>();
            return result;
        }

        protected WebStoreCatalog CreateCatalog()
        {
            var accessor = CreateCatalogAccessor();
            var catalog = new WebStoreCatalog()
            {
                Name = "UNIT TEST CATALOG",
                SellerName = "UNIT TEST SELLER",
                IsApproved = true,
                Description = "UNIT TEST DESCRIPTION",
            };
            var saved = accessor.SaveCatalog(catalog);
            return saved;
        }
    }
}
