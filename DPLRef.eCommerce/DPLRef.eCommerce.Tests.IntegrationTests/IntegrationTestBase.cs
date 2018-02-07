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
            DbContextFactory.CreateGlobalContext();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            DbContextFactory.CancelGlobalTransaction();
        }
    }
}
