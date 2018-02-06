using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Engines.Remmitance
{

    public interface IRemittanceCalculationEngine : IServiceContractBase
    {
        RemittanceCalculationResult CalculateFee(int sellerId, decimal total);
    }
}
