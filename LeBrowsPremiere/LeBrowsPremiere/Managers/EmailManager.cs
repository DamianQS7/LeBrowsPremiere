using LeBrowsPremiere.Models;
using System.Net.Mail;
using System.Net;

namespace LeBrowsPremiere.Managers
{
    public static class EmailManager
    {
        public static async Task SendEmail(EmailConfigurationModel emailConfiguration, ClientEmailInformationModel clientEmailInfo)
        {
            try
            {
                MailMessage msg = CreateMessage(emailConfiguration, clientEmailInfo);
                //msg.IsBodyHtml = true;

                SmtpClient smtp = CreateSmtpClient(emailConfiguration);
                await smtp.SendMailAsync(msg);
            }
            catch (Exception e){
                //Invalid smtp setup
                var message = e.InnerException;
            }
        }

        public static SmtpClient CreateSmtpClient(EmailConfigurationModel emailConfiguration)
        {
            SmtpClient smtp = new SmtpClient(emailConfiguration.SmtpClientHost, emailConfiguration.SmtpClientPort);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(emailConfiguration.NetworkCredentialEmail, emailConfiguration.NetworkCredentialPassword);
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            return smtp;
        }

        public static MailMessage CreateMessage(EmailConfigurationModel emailConfiguration, ClientEmailInformationModel clientEmailInfo)
        {
            MailMessage msg = new();

            msg.From = new MailAddress(emailConfiguration.NetworkCredentialEmail, "LeBrows Premiere");
            msg.To.Add(new MailAddress(clientEmailInfo.Email, $"{clientEmailInfo.FirstName} {clientEmailInfo.LastName}"));
            msg.Subject = clientEmailInfo.MessageSubject;
            msg.Body = clientEmailInfo.MessageBody;
            return msg;
        }
    }
}
