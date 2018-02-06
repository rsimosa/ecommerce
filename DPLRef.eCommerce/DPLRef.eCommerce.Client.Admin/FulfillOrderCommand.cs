using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Contracts.Admin.Fulfillment;
using DPLRef.eCommerce.Managers;

namespace DPLRef.eCommerce.Client.Admin
{
    class FulfillOrderCommand :BaseUICommand
    {
        public FulfillOrderCommand(AmbientContext ambientContext)
            : base(ambientContext)
        {
        }

        public override string Name => "Fulfill Order";

        protected override UICommandParameter[] Parameters { get; set; } = new UICommandParameter[]
        {
            new IntUICommandParameter()
            {
                Message = "Order ID",
            },

        };

        protected override void CallManager()
        {
            var managerFactory = new ManagerFactory(Context);
            var fulfillmentManager = managerFactory.CreateManager<IAdminFulfillmentManager>();

            var response = fulfillmentManager.FulfillOrder((Parameters[0] as IntUICommandParameter).Value);
            ShowResponse(response, response.Message);
        }
    }
}
