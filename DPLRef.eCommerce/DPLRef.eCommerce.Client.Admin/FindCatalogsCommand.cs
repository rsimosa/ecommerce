using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Contracts.Admin.Catalog;
using DPLRef.eCommerce.Managers;

namespace DPLRef.eCommerce.Client.Admin
{
    class FindCatalogsCommand : BaseUICommand
    {
        public FindCatalogsCommand(AmbientContext ambientContext)
          : base(ambientContext)
        {
        }

        public override string Name => "Find Catalogs";

        protected override void CallManager()
        {
            var managerFactory = new ManagerFactory(Context);
            var webStoreCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();

            var response = webStoreCatalogManager.FindCatalogs();
            ShowResponse(response, StringUtilities.DataContractToJson<WebStoreCatalog[]>(response.Catalogs));
        }
    }
}
