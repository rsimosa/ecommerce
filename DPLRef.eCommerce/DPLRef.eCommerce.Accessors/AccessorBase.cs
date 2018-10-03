using System;
using DPLRef.eCommerce.Common.Shared;
using DPLRef.eCommerce.Utilities;
using Microsoft.Extensions.Configuration;

namespace DPLRef.eCommerce.Accessors
{
    abstract class AccessorBase : ServiceContractBase
    {
        public UtilityFactory UtilityFactory { get; set; }
    }
}
