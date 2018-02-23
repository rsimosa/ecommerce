using System.Transactions;
using DPLRef.eCommerce.Accessors.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DPLref.eCommerce.Tests.IntegrationTests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [TestClass]
    public abstract class IntegrationTestBase
    {
        public IntegrationTestBase()
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
    }
}
