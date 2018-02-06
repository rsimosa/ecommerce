using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.Admin.Sales
{
    [DataContract]
    public class SalesTotalsResponse : ResponseBase
    {
        [DataMember]
        public int OrderCount { get; set; }
        [DataMember]
        public decimal OrderTotal { get; set; }
    }
}
