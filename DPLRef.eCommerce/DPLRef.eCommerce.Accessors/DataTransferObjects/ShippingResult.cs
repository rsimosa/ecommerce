using System.Runtime.Serialization;

namespace DPLRef.eCommerce.Accessors.DataTransferObjects
{
    [DataContract]
    public class ShippingResult
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string ShippingProvider { get; set; }

        [DataMember]
        public string TrackingCode { get; set; }
    }
}
