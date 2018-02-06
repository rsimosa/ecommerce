using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Contracts.Admin.Catalog;
using DPLRef.eCommerce.Managers;

namespace DPLRef.eCommerce.Client.Admin
{
    class QueryWithBadTokenCommand : BaseUICommand
    {
        public QueryWithBadTokenCommand(AmbientContext ambientContext)
          : base(ambientContext)
        {
        }

        public override string Name => "Find Catalogs with Bad Token";

        protected override void CallManager()
        {
            var badContext = new AmbientContext()
            {
                SellerId = Context.SellerId,
                AuthToken = null
            };
            var managerFactory = new ManagerFactory(badContext);
            var webStoreCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();

            var response = webStoreCatalogManager.FindCatalogs();
            ShowResponse(response, StringUtilities.DataContractToJson<WebStoreCatalog[]>(response.Catalogs));
        }
    }
}
