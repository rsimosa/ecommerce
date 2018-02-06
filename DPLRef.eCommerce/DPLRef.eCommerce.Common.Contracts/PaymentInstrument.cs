using System.Runtime.Serialization;

namespace DPLRef.eCommerce.Common.Contracts
{
    /// <summary>
    /// The data contract will contains the details of the payment method to be used fore the transaction
    /// </summary>
    [DataContract]
    public class PaymentInstrument
    {
        [DataMember]
        public PaymentTypes PaymentType { get; set; }
         
        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string ExpirationDate { get; set; }

        [DataMember]
        public int? VerificationCode { get; set; }
    }

    public enum PaymentTypes
    {
        CreditCard
    }
}
