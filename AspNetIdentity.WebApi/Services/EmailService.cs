using Microsoft.AspNet.Identity;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace AspNetIdentity.WebApi.Services
{
    public class EmailServicePaperCut : IIdentityMessageService
    {


        public async Task SendAsync(IdentityMessage message)
        {
            await configSendPaperCutAsync(message);
        }

        private async Task configSendPaperCutAsync(IdentityMessage message)
        {

            SmtpClient client = new SmtpClient("127.0.0.1", 25);    // papercut is listening here...
            MailAddress fa = new MailAddress("admin@misra.com", "Admin " + (char)0xD8 + " Email", System.Text.Encoding.UTF8);

            MailAddress ta = new MailAddress(message.Destination);
            MailMessage mymessage = new MailMessage(fa, ta);

            mymessage.Body = message.Body;
            mymessage.Subject = message.Subject;

            await client.SendMailAsync(mymessage);
            
            mymessage.Dispose();
        }


        
    }


    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();

            myMessage.AddTo(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress("taiseer@bitoftech.net", "Taiseer Joudeh");
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"], 
                                                    ConfigurationManager.AppSettings["emailService:Password"]);

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                await transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                //Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }
        }
    }
}