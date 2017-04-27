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
        

        public async void SendEmailAsync(string email, string qrcode, string badgenum)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Jake Visser", "jake@woofy.io"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Your Queercon Group Buy";

            var builder = new BodyBuilder();
            string message = "<p>Thank you for your purchase of " + badgenum + " badge(s). Stripe should have sent you a recipt, and you should have a copy of the QR Code link in your inbox as well.</p>" +
"<p><b> Treat this code as cash.The first person to collect a badge using it gets to keep it. If you think your code has been stolen contact us from your email address with the relevent details and we will work to reset it </b></p>" +
"<p>This is your URL: <a href=\"" + Startup.uri + "home/img?qrtext=" + qrcode + "\"> " + Startup.uri + "home/img?qrtext=" + qrcode + " </a></p>";

            builder.HtmlBody = string.Format(message);

            emailMessage.Body = builder.ToMessageBody();


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
