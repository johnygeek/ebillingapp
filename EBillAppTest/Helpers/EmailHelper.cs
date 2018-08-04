using EBillAppTest.Models;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace EBillAppTest.Helpers
{
    public class EmailHelper
    {
        public static async Task SendEmailAsync(string message,string from, string to)
        {
            MailMessage mail = new MailMessage(from, to);
            using (var client = new SmtpClient())
            {
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.gmail.com";
                client.Credentials = new NetworkCredential("salil.contractor@gmail.com", "salil@2009$");
                mail.Subject = "Your Order";
                mail.Body = message;
                mail.IsBodyHtml = true;
                await client.SendMailAsync(mail);
            }
        }
    }
}