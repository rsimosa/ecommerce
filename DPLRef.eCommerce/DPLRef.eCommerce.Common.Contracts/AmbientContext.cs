using System;
using System.Runtime.Serialization;

namespace DPLRef.eCommerce.Common.Contracts
{
    [DataContract]
    public class AmbientContext
    {
        [DataMember]
        public string AuthToken { get; set; }

        [DataMember]
        public Guid SessionId { get; set; }

        [DataMember]
        public int SellerId { get; set; }
    }
}
