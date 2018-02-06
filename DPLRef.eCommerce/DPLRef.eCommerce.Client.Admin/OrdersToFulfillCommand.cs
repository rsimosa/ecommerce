using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Contracts.Admin.Fulfillment;
using DPLRef.eCommerce.Managers;

namespace DPLRef.eCommerce.Client.Admin
{
    class OrdersToFulfillCommand : BaseUICommand
    {
        public OrdersToFulfillCommand(AmbientContext ambientContext)
            : base(ambientContext)
        {
        }

        public override string Name => "Orders to Fulfill";

        protected override UICommandParameter[] Parameters { get; set; } = new UICommandParameter[] { };
        protected override void CallManager()
        {
            var managerFactory = new ManagerFactory(Context);
            var fulfillmentManager = managerFactory.CreateManager<IAdminFulfillmentManager>();

            var response = fulfillmentManager.GetOrdersToFulfill();
            if (response.Orders.Length > 0)
            {
                foreach (var order in response.Orders)
                {
                    ShowResponse(response, $"Order Id: {order.Id}, Order Status: {order.Status}");
                }
            }
            else
            {
                ShowResponse(response, "No orders found to fulfill");
            }
        }
    }
}
