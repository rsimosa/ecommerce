using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Utilities;
using DPLRef.eCommerce.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using DPLRef.eCommerce.Accessors.Catalog;
using DPLRef.eCommerce.Accessors.Notifications;
using DPLRef.eCommerce.Accessors.Sales;
using DPLRef.eCommerce.Accessors.Remittance;

namespace DPLRef.eCommerce.Accessors
{
    public class AccessorFactory : FactoryBase
    {
        private UtilityFactory _utilityFactory;

        public AccessorFactory(AmbientContext context, UtilityFactory utilityFactory) 
            : base(context)
        {
            // NOTE: this is here to ensure the factories from the Manager are propogated down to the other factories 
            _utilityFactory = utilityFactory ?? new UtilityFactory(Context);

            AddType<ICartAccessor>(typeof(CartAccessor));
            AddType<ICatalogAccessor>(typeof(CatalogAccessor));
            AddType<IEmailAccessor>(typeof(EmailAccessor));
            AddType<IOrderAccessor>(typeof(OrderAccessor));
            AddType<IEmailAccessor>(typeof(EmailAccessor));
            AddType<IPaymentAccessor>(typeof(PaymentAccessor));
            AddType<IShippingAccessor>(typeof(ShippingAccessor));
            AddType<ISellerAccessor>(typeof(SellerAccessor));
            AddType<IRemittanceAccessor>(typeof(RemittanceAccessor));
            AddType<IShippingRulesAccessor>(typeof(ShippingRulesAccessor));
        }

        public T CreateAccessor<T>() where T : class
        {
            return CreateAccessor<T>(null);
        }
        public T CreateAccessor<T>(UtilityFactory utilityFactory) where T : class
        {
            _utilityFactory = utilityFactory ?? _utilityFactory;

            T result = base.GetInstanceForType<T>();


            if (_utilityFactory == null)
            {
                _utilityFactory = new UtilityFactory(Context);
            }


            // Configure the context if the result is not a mock
            if (result is AccessorBase)
            {
                (result as AccessorBase).Context = Context;
                (result as AccessorBase).UtilityFactory = _utilityFactory;
            }

            return result;
        }
    }
}
