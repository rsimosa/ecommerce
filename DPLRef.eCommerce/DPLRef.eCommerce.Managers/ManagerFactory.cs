using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Engines;
using System;
using DPLRef.eCommerce.Contracts.Admin.Catalog;
using DPLRef.eCommerce.Contracts.Admin.Sales;
using DPLRef.eCommerce.Contracts.BackOfficeAdmin.Remittance;
using DPLRef.eCommerce.Contracts.WebStore.Catalog;
using DPLRef.eCommerce.Contracts.WebStore.Sales;
using DPLRef.eCommerce.Managers.Notification;
using DPLRef.eCommerce.Managers.Remittance;
using DPLRef.eCommerce.Managers.Sales;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Contracts.Admin.Fulfillment;
using DPLRef.eCommerce.Contracts.ServiceHost.Notifications;
using DPLRef.eCommerce.Utilities;

namespace DPLRef.eCommerce.Managers
{
    public class ManagerFactory : FactoryBase
    {
        private AccessorFactory _accessorFactory;
        private UtilityFactory _utilityFactory;
        private EngineFactory _engineFactory;

        public ManagerFactory(AmbientContext context, EngineFactory engineFactory = null, AccessorFactory accessorFactory = null, UtilityFactory utilityFactory = null) 
            : base(context)
        {

            // NOTE: this is here to ensure the factories from the Manager are propogated down to the other factories 
            _utilityFactory = utilityFactory ?? new UtilityFactory(Context);
            _accessorFactory = accessorFactory ?? new AccessorFactory(Context, _utilityFactory);
            _engineFactory = engineFactory ?? new EngineFactory(Context, _accessorFactory, _utilityFactory);

            AddType<IWebStoreCartManager>(typeof(OrderManager));
            AddType<IWebStoreOrderManager>(typeof(OrderManager));
            AddType<IReturnsManager>(typeof(OrderManager));
            AddType<IWebStoreCatalogManager>(typeof(Catalog.CatalogManager));
            AddType<IAdminCatalogManager>(typeof(Catalog.CatalogManager));
            AddType<INotificationManager>(typeof(NotificationManager));
            AddType<IBackOfficeRemittanceManager>(typeof(RemittanceManager));
            AddType<IAdminRemittanceManager>(typeof(RemittanceManager));
            AddType<IAdminFulfillmentManager>(typeof(OrderManager));
        }

        public T CreateManager<T>() where T : class
        {
            T result = CreateManager<T>(null, null, null);
            return result;
        }

        public T CreateManager<T>(
            EngineFactory engineFactory, AccessorFactory accessorFactory, UtilityFactory utilityFactory) where T : class
        {
            _engineFactory = engineFactory ?? _engineFactory;
            _accessorFactory = accessorFactory ?? _accessorFactory;
            _utilityFactory = utilityFactory ?? _utilityFactory;

            if (Context == null)
            {
                throw new InvalidOperationException("Context cannot be null");
            }

            if (_utilityFactory == null)
            {
                _utilityFactory = new UtilityFactory(Context);
            }

            if (accessorFactory == null)
            {
                _accessorFactory = new AccessorFactory(Context, _utilityFactory);
            }

            if (engineFactory == null)
            {
                _engineFactory = new EngineFactory(Context, _accessorFactory, _utilityFactory);
            }

            T result = GetInstanceForType<T>();

            if (result is ManagerBase)
            {
                (result as ManagerBase).Context = Context;
                (result as ManagerBase).EngineFactory = _engineFactory;
                (result as ManagerBase).AccessorFactory = _accessorFactory;
                (result as ManagerBase).UtilityFactory = _utilityFactory;
            }
            else
                // mocking of the manager factory is not supported so every result should implement ManagerBase
                throw new InvalidOperationException($"{typeof(T).Name} does not implement ManagerBase");

            return result;
        }
    }
}
