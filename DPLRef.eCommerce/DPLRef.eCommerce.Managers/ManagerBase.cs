using DPLRef.eCommerce.Accessors;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Engines;
using DPLRef.eCommerce.Utilities;

namespace DPLRef.eCommerce.Managers
{
    abstract class ManagerBase : ServiceContractBase
    {
        public EngineFactory EngineFactory { get; set; }
        public AccessorFactory AccessorFactory { get; set; }
        public UtilityFactory UtilityFactory { get; set; } 

        protected ManagerBase()
        {

        }
    }
}
