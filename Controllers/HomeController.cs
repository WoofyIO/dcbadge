using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using QRCoder;

namespace dcbadge.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {



            return View();
        }

        public IActionResult qrtest()
        {
            string thetext = "Testing the QR Code";
            ViewData["Message"] = thetext;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(thetext, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);

            string base64txt = Convert.ToBase64String(qrCodeImage);

            string mailstring = "<b>Testing</b> again";

            Helpers.Mailer mailer = new Helpers.Mailer();
            S
            mailer.SendEmailAsync("jake@woofy.io", "This is a test2", mailstring);





            ViewData["Image"] = base64txt;

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Charge(string stripeEmail, string stripeToken)
        {
            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();

            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            var charge = charges.Create(new StripeChargeCreateOptions
            {
                Amount = 500,
                Description = "QC-DCBadgeOrder",
                Currency = "usd",
                CustomerId = customer.Id
 
            });

            ViewData["Message"] = "CustomerID: " + customer.Id + " Email: " + stripeEmail + " ChargeID: " + charge.Id + " " + charge.Amount/100 + " " + charge.Status;

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
