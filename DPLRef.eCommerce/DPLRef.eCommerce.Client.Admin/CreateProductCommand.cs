using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Contracts.Admin.Catalog;
using DPLRef.eCommerce.Managers;

namespace DPLRef.eCommerce.Client.Admin
{
    class CreateProductCommand : BaseUICommand
    {
        public CreateProductCommand(AmbientContext ambientContext)
          : base(ambientContext)
        {
        }

        public override string Name
        {
            get { return "Create Product"; }
        }

        protected override UICommandParameter[] Parameters { get; set; } = new UICommandParameter[]
        {
             new IntUICommandParameter()
            {
                Message = "Catalog ID",
            },
            new StringUICommandParameter()
            {
                Message = "Product Name",
            },
            new DecimalUICommandParameter()
            {
                Message = "Product Price",
            },
        };

        protected override void CallManager()
        {
            var managerFactory = new ManagerFactory(Context);
            var webStoreCatalogManager = managerFactory.CreateManager<IAdminCatalogManager>();

            var product = new Product()
            {
                Name = (Parameters[1] as StringUICommandParameter).Value,
                Price = (Parameters[2] as DecimalUICommandParameter).Value,
                Detail = "",
                CatalogId = (Parameters[0] as IntUICommandParameter).Value,
                Summary = "",
                ShippingWeight = 0
            };
            var response = webStoreCatalogManager.SaveProduct(product.CatalogId, product);
            ShowResponse(response, StringUtilities.DataContractToJson<Product>(response.Product));
        }
    }
}
