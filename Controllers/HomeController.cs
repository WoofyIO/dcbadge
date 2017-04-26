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

            ViewData["Message"] = "";

            ViewBag.RequestCode = Request.Cookies["RequestCode"];

            if(!string.IsNullOrEmpty(ViewBag.RequestCode))
            {
                ViewData["RequestCode"] = ViewBag.RequestCode;
            }
            else
            {
                ViewData["RequestCode"] = "";
            }

            return View();
        }

        public IActionResult Validate(string RequestCode)
        {
            Helpers.Sql sql = new Helpers.Sql();
            ViewData["Message"] = "";
            ViewData["Back"] = 1;
            ViewData["ShowPay"] = 0;
            ViewData["ShowRecover"] = 0;

            if (!string.IsNullOrEmpty(RequestCode))
            {
                Response.Cookies.Append("RequestCode", RequestCode);

                if (sql.verifyCode(RequestCode) == 1)
                {

                    if(sql.codeUsed(RequestCode) == false)
                    {
                        ViewData["Message"] = "Your code is good - lets pay";
                        ViewData["Back"] = 0;
                        ViewData["ShowPay"] = 1;
                        ViewData["MaxBadges"] = sql.maxBadges(RequestCode);


                    }
                    else
                    {

                        ViewData["Message"] = "Your code is allready used... ";
                        ViewData["Back"] = 0;
                        ViewData["ShowRecover"] = 1;

                    }


                }
                else
                {

                    ViewData["Message"] = "Invalid Code";
                    ViewData["Back"] = 1;
                }
                
            }

            return View();
        }

        public IActionResult Pay(int BadgeNumber)
        {

            ViewData["TotalPrice"] = "";
            ViewData["BadgeNumber"] = "";

            Helpers.Sql sql = new Helpers.Sql();

            ViewData["Message"] = "";
            ViewData["Back"] = 1;
            ViewData["ShowPay"] = 0;
            ViewData["ShowRecover"] = 0;

            ViewBag.RequestCode = Request.Cookies["RequestCode"];
            string RequestCode = ViewBag.RequestCode;

            if (!string.IsNullOrEmpty(RequestCode) && (BadgeNumber > 0) && ( BadgeNumber <= sql.maxBadges(RequestCode)))
            {
                if (sql.verifyCode(RequestCode) == 1)
                {
                    sql.updatePrice(RequestCode, BadgeNumber, (BadgeNumber * 270 * 100));
                    ViewData["TotalPrice"] = (sql.getPrice(RequestCode) / 100);
                    ViewData["BadgeNumber"] = BadgeNumber;
                    ViewData["ShowPay"] = 1;
                    ViewData["Back"] = 0;
                }
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

        public IActionResult Complete(string stripeEmail, string stripeToken)
        {

            ViewBag.RequestCode = Request.Cookies["RequestCode"];
            string RequestCode = ViewBag.RequestCode;

            Helpers.Sql sql = new Helpers.Sql();
            int price = sql.getPrice(RequestCode);

            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();


            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            var charge = charges.Create(new StripeChargeCreateOptions
            {
                Amount = price,
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
