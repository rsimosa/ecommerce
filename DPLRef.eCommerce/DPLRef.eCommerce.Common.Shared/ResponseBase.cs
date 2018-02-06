using System.Runtime.Serialization;

namespace DPLRef.eCommerce.Common.Shared
{
    public abstract class ResponseBase
    {
        public bool Success { get; set; }
        
        public string Message { get; set; } 
    }
}
