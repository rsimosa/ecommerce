using DPLRef.eCommerce.Accessors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPLRef.eCommerce.Accessors.Notifications;
using DPLRef.eCommerce.Utilities;

namespace DPLRef.eCommerce.Tests.AccessorTests
{
    [TestClass]
    public class EmailAccessorTests : DbTestAccessorBase
    {

        private IEmailAccessor CreateEmailAccessor()
        {
            var accessor = new AccessorFactory(Context, new UtilityFactory(Context));
            var result = accessor.CreateAccessor<IEmailAccessor>();
            return result;
        }

        
        [TestMethod]
        [TestCategory("Accessor Tests")]
        public void EmailAccessor_Send()
        {
            var accessor = CreateEmailAccessor();
            accessor.SendEmailNotification("test@dontpaniclabs.com", "eCommerce Email", "Body");
        }        
    }
}
