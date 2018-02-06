using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Contracts.BackOfficeAdmin.Remittance
{
    [DataContract]
    public class OrderDataResponse : ResponseBase
    {
        [DataMember]
        public SellerOrderData[] SellerOrderData { get; set; }
    }

    [DataContract]
    public class SellerOrderData
    {
        [DataMember]
        public int SellerId { get; set; }

        [DataMember]
        public string SellerName { get; set; }

        [DataMember]
        public int OrderCount { get; set; }

        [DataMember]
        public decimal OrderTotal { get; set; }

        [DataMember]
        public decimal FeeAmount { get; set; }

        [DataMember]
        public decimal RemittanceAmount { get; set; }
       
    }
}
