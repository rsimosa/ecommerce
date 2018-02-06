using DPLRef.eCommerce.Common.Shared;


namespace DPLRef.eCommerce.Accessors.Notifications
{
    public interface IEmailAccessor : IServiceContractBase
    {
        void SendEmailNotification(string emailTo, string subject, string body);
    }
}
