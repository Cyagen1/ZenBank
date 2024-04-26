using System.Net.Mail;
using ZenReporting.Contracts;

namespace ZenReporting.Services
{
    public class MailService : IMailService
    {
        private readonly MailServiceSettings _mailServiceSettings;

        public MailService(MailServiceSettings mailServiceSettings)
        {
            _mailServiceSettings = mailServiceSettings;
        }
        public void SendMail(MemoryStream stream, User user)
        {
            string senderMail = _mailServiceSettings.SenderMail;

            string recieverMail = user.Email;

            string smtpServer = _mailServiceSettings.SmtpServer;
            int smtpPort = _mailServiceSettings.SmtpPort;

            MailMessage message = new MailMessage(senderMail, recieverMail);

            message.Subject = "Daily transactions report";
            message.Body = "Here is your daily transactions report, please find the attached PDF.";

            Attachment attachment = new Attachment(stream, "report.pdf", "application/pdf");
            message.Attachments.Add(attachment);

            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.Send(message);
        }
    }
}
