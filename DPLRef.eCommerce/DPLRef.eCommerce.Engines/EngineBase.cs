using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Utilities;

namespace DPLRef.eCommerce.Engines
{
    abstract class EngineBase : ServiceContractBase
    {
        public AccessorFactory AccessorFactory { get; set; }
        public UtilityFactory UtilityFactory { get; set; }

        protected EngineBase()
        {
            
        }
    }
}
