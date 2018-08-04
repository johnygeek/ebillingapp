using EBillAppTest.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace EBillAppTest.Helpers
{
    public class EmailHelper
    {
        public static async Task SendEmailAsync(string message, string from, string to, string orderNumber)
        {
            MailMessage message1 = new MailMessage(from, to);
            message1.From = new MailAddress(from);
            string fileName1 = HttpContext.Current.Server.MapPath("~/content/images/logo.jpg");
            string fileName2 = HttpContext.Current.Server.MapPath("~/content/images/facebook.png");
            string fileName3 = HttpContext.Current.Server.MapPath("~/content/images/instagram.jpg");
            Attachment attachment1 = new Attachment(fileName1);
            Attachment attachment2 = new Attachment(fileName2);
            Attachment attachment3 = new Attachment(fileName3);
            attachment1.ContentId = "logo";
            attachment2.ContentId = "facebook";
            attachment3.ContentId = "instagram";
            message1.Body = "<html><body><p align='center'><img src = 'cid:" + attachment1.ContentId + "' /></p><hr />" + message + "<hr/><a href = '" + Settings.FacebookConnect + "'><img style = 'width:30px;height:30px;' src = 'cid:" + attachment2.ContentId + "' /></a><a href = '" + Settings.InstagramConnect + "' ><img style = 'width:30px;height:30px;' src = 'cid:" + attachment3.ContentId + "' /></a ><br/><a href = '" + Settings.FacebookConnect + "' >Follow us on Facebook</a><br/><br/><a href = '" + Settings.InstagramConnect + "' >Follow us on Instagram</a></body></html>";
            message1.Attachments.Add(attachment1);
            message1.Attachments.Add(attachment2);
            message1.Attachments.Add(attachment3);
            SmtpClient client = new SmtpClient();
            try
            {
                int result;
                int.TryParse(Settings.Port, out result);
                client.Port = result;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Host = Settings.SMTPServer;
                client.Credentials = (ICredentialsByHost)new NetworkCredential(Settings.UserId, Settings.Password);
                message1.Subject = "Your CraveTheShake Order #" + orderNumber;
                message1.IsBodyHtml = true;
                try
                {
                    await client.SendMailAsync(message1);
                }
                catch (Exception ex)
                {
                }
            }
            finally
            {
                client?.Dispose();
            }
            client = (SmtpClient)null;
        }
    }
}