using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Contracts.Admin.Catalog;
using DPLRef.eCommerce.Managers;

namespace DPLRef.eCommerce.Client.Admin
{
    class CreateCatalogCommand : BaseUICommand
    {
        public CreateCatalogCommand(AmbientContext ambientContext)
          : base(ambientContext)
        {
        }

        public override string Name
        {
            get { return "Create Catalog"; }
        }

        protected override UICommandParameter[] Parameters { get; set; } = new UICommandParameter[]
        {
            new StringUICommandParameter()
            {
                Message = "Catalog Name",
            },
        };

        protected override void CallManager()
        {
            var managerFactory = new ManagerFactory(Context);
            var webStoreCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();

            var catalog = new WebStoreCatalog()
            {
                Name = (Parameters[0] as StringUICommandParameter).Value,
                Description = "new catalog"
            };
            var response = webStoreCatalogManager.SaveCatalog(catalog);
            ShowResponse(response, StringUtilities.DataContractToJson<WebStoreCatalog>(response.Catalog));
        }
    }
}
