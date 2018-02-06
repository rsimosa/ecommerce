using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DPLRef.eCommerce.Engines.Remmitance
{
    [DataContract]
    public class RemittanceCalculationResult
    {
        [DataMember]
        public decimal FeeAmount { get; set; }

        [DataMember]
        public decimal RemittanceAmount { get; set; }
    }
}
