using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Contracts.Admin.Catalog;
using DPLRef.eCommerce.Contracts.Admin.Sales;
using DPLRef.eCommerce.Managers;

namespace DPLRef.eCommerce.Client.Admin
{
    class TotalsCommand : BaseUICommand
    {
        public TotalsCommand(AmbientContext ambientContext)
          : base(ambientContext)
        {
        }

        public override string Name => "Sales Totals";


        protected override UICommandParameter[] Parameters { get; set; } = new UICommandParameter[] { };
        protected override void CallManager()
        {
            var managerFactory = new ManagerFactory(Context);
            var remittanceManager = managerFactory.CreateManager<IAdminRemittanceManager>();

            var response = remittanceManager.Totals();
            ShowResponse(response, $"OrderCount: {response.OrderCount}, OrderTotal: {response.OrderTotal}");
        }
    }
}
