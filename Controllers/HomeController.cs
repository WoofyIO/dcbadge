using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace dcbadge.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            ViewBag.isSet = Request.Cookies["isSet"];

            if(ViewBag.isSet == "true")
            {
                ViewData["Message"] = "true";
            }
            else
            {
                ViewData["Message"] = "false";
            }

            return View();
        }

        public IActionResult qrtest()
        {
            string thetext = "Testing the QR Code";
            ViewData["Message"] = thetext;


            Helpers.QRGen qrcode64 = new Helpers.QRGen();
            string base64txt = qrcode64.genQRCode64(thetext);

            ViewData["Image"] = base64txt;

            string mailstring = "<b>Testing</b> again </br> <p></p>";
            Helpers.Mailer mailer = new Helpers.Mailer();
            mailer.SendEmailAsync("jake@woofy.io", "This is a test2", mailstring);

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            Response.Cookies.Append("isSet", "true");

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

        public IActionResult img(string qrtext)
        {

            if(qrtext == null)
            {
                qrtext = "blank";
            }

            Helpers.QRGen qrcode64 = new Helpers.QRGen();
            byte[] qrcode = qrcode64.genQRCodeByte(qrtext);
            return File(qrcode, "image/png");
        }
    }
}
