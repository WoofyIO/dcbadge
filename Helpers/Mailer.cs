using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;

namespace dcbadge.Helpers
{
    public class Mailer
    {
        

        public async void SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Jake Visser", "jake@woofy.io"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("text/html") { Text = message };

            using (var client = new SmtpClient())
            {
                // client.LocalDomain = "some.domain.com";
                // await client.ConnectAsync("smtp.woofy.io", 25, SecureSocketOptions.None).ConfigureAwait(false);
                //  await client.SendAsync(emailMessage).ConfigureAwait(false);
                //  await client.DisconnectAsync(true).ConfigureAwait(false);

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync("smtp.woofy.io", 25, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                //client.Authenticate("test", "password");

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
