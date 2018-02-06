using System.Runtime.Serialization;

namespace DPLRef.eCommerce.Common.Contracts
{
    [DataContract]
    public class Address
    {
        [DataMember]
        public string First { get; set; }
        [DataMember]
        public string Last { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string Addr1 { get; set; }
        [DataMember]
        public string Addr2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Postal { get; set; }
    }
}
