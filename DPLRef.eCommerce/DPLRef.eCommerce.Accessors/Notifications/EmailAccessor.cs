using System;

namespace DPLRef.eCommerce.Accessors.Notifications
{
    class EmailAccessor : AccessorBase, IEmailAccessor
    {
        public void SendEmailNotification(string emailTo, string subject, string body)
        {
            // Do nothing, this is just a mock.
#if DEBUG
            // write the email contents to the console
            Console.WriteLine("Email sent!");
            Console.WriteLine($"Recipient: {emailTo}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine("Email Body:");
            Console.WriteLine(body);
#endif
        }
    }
}
