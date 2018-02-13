using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Utilities
{
    public class UtilityFactory : FactoryBase
    {
        public UtilityFactory(AmbientContext context) : base(context)
        {
            AddType<ISecurityUtility>(typeof(SecurityUtility));
            AddType<IAddressUtility>(typeof(AddressUtility));
            AddType<IAsyncUtility>(typeof(AsyncUtility));
        }

        public T CreateUtility<T>() where T : class
        {
            T result = base.GetInstanceForType<T>();

            // Configure the context if the result is not a mock
            if (result is UtilityBase)
                (result as UtilityBase).Context = Context;

            return result;
        }
    }
}
