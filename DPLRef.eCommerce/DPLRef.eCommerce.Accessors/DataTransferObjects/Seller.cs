using System.Runtime.Serialization;

namespace DPLRef.eCommerce.Accessors.DataTransferObjects
{
    [DataContract]
    public class Seller
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string OrderNotificationEmail { get; set; }

        [DataMember]
        public int BankRoutingNumber { get; set; }

        [DataMember]
        public int BankAccountNumber { get; set; }

        [DataMember]
        public bool IsApproved { get; set; }

        [DataMember]
        public string UserName { get; set; }
    }
}
