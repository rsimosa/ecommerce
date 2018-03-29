using DPLRef.eCommerce.Contracts.ServiceHost.Notifications;
using DPLRef.eCommerce.Managers;
using DPLRef.eCommerce.Utilities;
using System;
using System.Threading;

namespace DPLRef.eCommerce.Service.Notifications
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Notifications Service");

            while (true)
            {
                var utilityFactory = new Utilities.UtilityFactory(new Common.Contracts.AmbientContext());
                var asyncUtility = utilityFactory.CreateUtility<IAsyncUtility>();
                var item = asyncUtility.CheckForNewItem();
                if (item != null)
                {
                    var managerFactory = new ManagerFactory(new Common.Contracts.AmbientContext());
                    var notificationManager = managerFactory.CreateManager<INotificationManager>();

                    if (item.EventType == Common.Contracts.AsyncEventTypes.OrderSubmitted)
                    {
                        notificationManager.SendNewOrderNotices(item.EventId);
                    }
                    if (item.EventType == Common.Contracts.AsyncEventTypes.OrderShipped)
                    {
                        notificationManager.SendOrderFulfillmentNotices(item.EventId);
                    }
                }

                Thread.Sleep(5000); // sleep 5 seconds
            }
        }
    }
}
