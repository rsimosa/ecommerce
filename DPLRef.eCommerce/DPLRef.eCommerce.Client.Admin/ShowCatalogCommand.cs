using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Contracts.Admin.Catalog;
using DPLRef.eCommerce.Managers;

namespace DPLRef.eCommerce.Client.Admin
{
    class ShowCatalogCommand : BaseUICommand
    {
        public ShowCatalogCommand(AmbientContext ambientContext)
          : base(ambientContext)
        {
        }

        public override string Name => "Show Catalog";

        protected override UICommandParameter[] Parameters { get; set; } = new UICommandParameter[]
        {
            new IntUICommandParameter()
            {
                Message = "Catalog ID",
            },
        };

        protected override void CallManager()
        {
            var managerFactory = new ManagerFactory(Context);
            var webStoreCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();

            var catalogId = (Parameters[0] as IntUICommandParameter).Value;            

            var response = webStoreCatalogManager.ShowCatalog(catalogId);
            ShowResponse(response, StringUtilities.DataContractToJson<WebStoreCatalog>(response.Catalog));
        }
    }
}
