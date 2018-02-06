using System.Runtime.Serialization;

namespace DPLRef.eCommerce.Accessors.DataTransferObjects
{
    [DataContract]
    public class PaymentCaptureResult
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
