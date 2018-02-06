using System.Runtime.Serialization;

namespace DPLRef.eCommerce.Accessors.DataTransferObjects
{
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
    }
}
