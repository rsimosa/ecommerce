using System;
using DPLRef.eCommerce.Accessors.Notifications;
using DPLRef.eCommerce.Accessors.Remittance;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Contracts.ServiceHost.Notifications;
using DPLRef.eCommerce.Engines.Notification;

namespace DPLRef.eCommerce.Managers.Notification
{
    class NotificationManager : ManagerBase, INotificationManager
    {
        #region IServiceContractBase
        public override string TestMe(string input)
        {
            input = base.TestMe(input);
            input = EngineFactory.CreateEngine<IEmailFormattingEngine>().TestMe(input);
            input = AccessorFactory.CreateAccessor<IEmailAccessor>().TestMe(input);

            return input;
        }

        #endregion

        public NotificationResponse SendNewOrderNotices(int orderId)
        {
            return SendOrderEventEmail(orderId, "New Order Notice");
        }

        public NotificationResponse SendOrderFulfillmentNotices(int orderId)
        {
            return SendOrderEventEmail(orderId, "Shipping Notice");

        }

        private NotificationResponse SendOrderEventEmail(int orderId, string eventMessage)
        {
            try
            {
                // get the order info
                var order = AccessorFactory.CreateAccessor<IOrderAccessor>()
                    .FindOrder(orderId);
                // get any necessary seller data
                var seller = AccessorFactory.CreateAccessor<ISellerAccessor>()
                    .Find(order.SellerId);
                // format the email
                var messageBody = EngineFactory.CreateEngine<IEmailFormattingEngine>()
                    .FormatOrderEmailBody(order, seller);
                // send the email
                AccessorFactory.CreateAccessor<IEmailAccessor>()
                    .SendEmailNotification(order.BillingAddress.EmailAddress, $"{eventMessage}: {seller.Name}", messageBody);

                return new NotificationResponse()
                {
                    Success = true,
                    Message = $"{eventMessage} email sent to {order.BillingAddress.EmailAddress} for order {order.Id}"
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                return new NotificationResponse()
                {
                    Success = false,
                    Message = $"There was a problem processing the {eventMessage} notice"
                };
            }

        }
    }
}
