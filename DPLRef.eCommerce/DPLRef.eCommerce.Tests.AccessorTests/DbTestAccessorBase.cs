using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Accessors.Catalog;
using DPLRef.eCommerce.Accessors.DataTransferObjects;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DPLRef.eCommerce.Accessors.EntityFramework;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Accessors.Remittance;
using System.Transactions;

namespace DPLRef.eCommerce.Tests.AccessorTests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [TestClass]
    public abstract class DbTestAccessorBase
    {
        public DbTestAccessorBase()
        {
        }

        [TestInitialize()]
        public void Init()
        {
            CreateGlobalContext();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            CancelGlobalTransaction();
        }

        public static void CreateGlobalContext()
        {
            eCommerceDbContext.UnitTestContext = eCommerceDbContext.Create(false);
            eCommerceDbContext.UnitTestContext.Database.BeginTransaction();
        }

        public static void CancelGlobalTransaction()
        {
            if (eCommerceDbContext.UnitTestContext != null)
            {
                eCommerceDbContext.UnitTestContext.Database.RollbackTransaction();
                eCommerceDbContext.UnitTestContext.AllowDispose = true;
                eCommerceDbContext.UnitTestContext.Dispose();
                eCommerceDbContext.UnitTestContext = null;
            }
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
