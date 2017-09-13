using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
namespace WeirdEnsemble2
{
    internal class SendGridEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            string sendGridApiKey = System.Configuration.ConfigurationManager.AppSettings["SendGrid.ApiKey"];
            SendGrid.SendGridClient client = new SendGrid.SendGridClient(sendGridApiKey);
            SendGrid.Helpers.Mail.SendGridMessage sendgridMessage = new SendGrid.Helpers.Mail.SendGridMessage();
            sendgridMessage.AddTo(message.Destination);
            sendgridMessage.Subject = message.Subject;
            sendgridMessage.SetFrom("no-reply@weirdensemble.com", "WeirdEnsemble.com Administrator");
            sendgridMessage.AddContent("text/html", message.Body);
            sendgridMessage.SetTemplateId("9f449d8f-c608-4336-9cbf-8a73ca391f3e");
            return client.SendEmailAsync(sendgridMessage);
        }
    }
}