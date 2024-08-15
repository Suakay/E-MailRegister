using MailKit.Security;
using MimeKit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
 
namespace MVC_Business.Servicess
{
    public class MailService : IMailService
    {
        public async Task SendMailAsync(string email, string subject, string message)
        {

            try

            {

                var newEmail = new MimeMessage();

                newEmail.From.Add(MailboxAddress.Parse("bengusuakay040@gmail.com"));

                newEmail.To.Add(MailboxAddress.Parse(email));

                newEmail.Subject = subject;

                var builder = new BodyBuilder();

                builder.HtmlBody = message;

                newEmail.Body = builder.ToMessageBody();

                var smtp = new SmtpClient();

                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("bengusuakay040@gmail.com", "ivomhkdyptmozugm");
                await smtp.SendAsync(newEmail);
                await smtp.DisconnectAsync(true);

            }

            catch (Exception ex)

            {

                throw new InvalidOperationException($"E POSTA GÖNDERİLİRKEN HATA OLUŞTU" + ex.Message);

            }

        }
    }
}
