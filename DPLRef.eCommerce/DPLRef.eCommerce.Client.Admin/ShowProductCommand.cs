using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Contracts.Admin.Catalog;
using DPLRef.eCommerce.Managers;

namespace DPLRef.eCommerce.Client.Admin
{
    class ShowProductCommand : BaseUICommand
    {
        public ShowProductCommand(AmbientContext ambientContext)
          : base(ambientContext)
        {
        }

        public override string Name => "Show Product";


        protected override UICommandParameter[] Parameters { get; set; } = new UICommandParameter[]
        {
            new IntUICommandParameter()
            {
                Message = "Catalog ID",
            },
            new IntUICommandParameter()
            {
                Message = "Product ID",
            },
        };

        protected override void CallManager()
        {
            var managerFactory = new ManagerFactory(Context);
            var webStoreCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();

            var catalogId = (Parameters[0] as IntUICommandParameter).Value;
            var productId = (Parameters[1] as IntUICommandParameter).Value;

            var response = webStoreCatalogManager.ShowProduct(catalogId, productId);
            ShowResponse(response, StringUtilities.DataContractToJson<Product>(response.Product));
        }
    }
}
