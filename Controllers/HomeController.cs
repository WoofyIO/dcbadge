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
            ViewData["RequestCode"] = RequestCode;

            if (!string.IsNullOrEmpty(RequestCode))
            {
                Response.Cookies.Append("RequestCode", RequestCode);

                if (sql.verifyCode(RequestCode) == true)
                {

                    if(sql.codeUsed(RequestCode) == false)
                    {
                        ViewData["Message"] = "Your code has been validated";
                        ViewData["Back"] = 0;
                        ViewData["ShowPay"] = 1;
                        ViewData["MaxBadges"] = sql.maxBadges(RequestCode);


                    }
                    else
                    {

                        ViewData["Message"] = "";
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

        public IActionResult Recover(string RequestCode, string email)
        {
            Helpers.Sql sql = new Helpers.Sql();
            Helpers.Mailer mail = new Helpers.Mailer();

            string[] data = sql.getRecover(RequestCode, email);

            if((!String.IsNullOrEmpty(data[0])) && (!String.IsNullOrEmpty(data[0])))
            {
                mail.SendEmailAsync(email, data[1], data[0]);
            }
            
            
            

            return View();
        }

        public IActionResult Pay(int BadgeNumber)
        {

            ViewData["TotalPrice"] = "0";
            ViewData["BadgeNumber"] = "0";

            Helpers.Sql sql = new Helpers.Sql();

            ViewData["Message"] = "";
            ViewData["Back"] = 1;
            ViewData["ShowPay"] = 0;
            ViewData["ShowRecover"] = 0;

            ViewBag.RequestCode = Request.Cookies["RequestCode"];
            string RequestCode = ViewBag.RequestCode;

            if (!string.IsNullOrEmpty(RequestCode) && (BadgeNumber > 0) && ( BadgeNumber <= sql.maxBadges(RequestCode)))
            {
                if (sql.verifyCode(RequestCode) == true)
                {
                    sql.updatePrice(RequestCode, BadgeNumber, (BadgeNumber * Startup.price));
                    ViewData["TotalPrice"] = sql.getPrice(RequestCode);
                    ViewData["BadgeNumber"] = BadgeNumber;
                    ViewData["ShowPay"] = 1;
                    ViewData["Back"] = 0;
                }
            }


                return View();
        }


        public IActionResult Complete(string stripeEmail, string stripeToken)
        {
            ViewData["Message"] = "If your seeing this, either you shouldnt be here, or something went wrong. Email us, or try again.";
            ViewData["Back"] = 1;
            ViewData["qrcode"] = "";
            ViewData["ShowEnd"] = 0;
            ViewData["TransError"] = "";
            ViewData["Image"] = "";
            ViewData["badgenum"] = "";
            ViewData["Email"] = "";
            ViewData["uri"] = Startup.uri;

            string qrcode = "";


            ViewBag.RequestCode = Request.Cookies["RequestCode"];
            string RequestCode = ViewBag.RequestCode;

            Helpers.Sql sql = new Helpers.Sql();
            Helpers.QRGen qr = new Helpers.QRGen();
            Helpers.Mailer mail = new Helpers.Mailer();

            int price = sql.getPrice(RequestCode);

            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();

            if(!sql.codeUsed(RequestCode))
            {
                if (!String.IsNullOrEmpty(stripeEmail) && !String.IsNullOrEmpty(stripeToken))
                {
                    try
                    {
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


                        if (String.Compare(charge.Status, "succeeded", true) == 0)
                        {
                            ViewData["Back"] = 0;
                            String guid = Guid.NewGuid().ToString();
                            qrcode = sql.getID(RequestCode) + ";" + guid;
                            sql.updateSale(RequestCode, stripeEmail, customer.Id, charge.Id, qrcode);
                            int badgenum = charge.Amount / Startup.price;
                            ViewData["badgenum"] = badgenum;
                            ViewData["qrcode"] = qrcode;
                            ViewData["ShowEnd"] = 1;
                            ViewData["Message"] = "";
                            ViewData["Image"] = qr.genQRCode64(qrcode);
                            ViewData["Email"] = stripeEmail;
                            mail.SendEmailAsync(stripeEmail, qrcode, badgenum.ToString());

                        }


                    }
                    catch (StripeException e)
                    {
                        ViewData["TransError"] = e.Message;
                        ViewData["Message"] = "Something went wrong... look at the error message, email us or try again.";
                    }
                }
            }

            

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Img(string qrtext)
        {

            if(qrtext == null)
            {
                qrtext = " ";
            }

            ViewData["Back"] = 1;

            Helpers.Sql sql = new Helpers.Sql();
            if (sql.verifyQR(qrtext))
            {
                ViewData["Back"] = 0;
            }

            Helpers.QRGen qr = new Helpers.QRGen();
            ViewData["qrcode"] = qrtext;
            ViewData["Image"] = qr.genQRCode64(qrtext);
            ViewData["uri"] = Startup.uri;

            return View();
        }

        public IActionResult Claim()
        {

            ViewData["Message"] = "";

            ViewBag.ClaimAdminCode = Request.Cookies["ClaimAdminCode"];

            if (!string.IsNullOrEmpty(ViewBag.ClaimAdminCode))
            {
                ViewData["ClaimAdminCode"] = ViewBag.ClaimAdminCode;
            }
            else
            {
                ViewData["ClaimAdminCode"] = "";
            }

            return View();
        }

        public IActionResult Claim2(string claimadmincode, string claimcode, string removeclaimcode)
        {
            Helpers.Sql sql = new Helpers.Sql();
            ViewData["Message"] = "";
            ViewData["Back"] = 1;
            ViewData["ShowClaim"] = 0;
            ViewData["CollectNumber"] = 0;
            ViewData["ShowValidate"] = 0;


            if (!string.IsNullOrEmpty(claimadmincode))
            {
                Response.Cookies.Append("ClaimAdminCode", claimadmincode);

            }
            else
            {
                ViewBag.ClaimAdminCode = Request.Cookies["ClaimAdminCode"];
                claimadmincode = ViewBag.ClaimAdminCode;
            }

            ViewBag.ClaimAdminCode = Request.Cookies["ClaimAdminCode"];

            

            if (String.Compare("handler", claimadmincode, true) == 0)
            {
                ViewData["Back"] = 0;
                ViewData["Message"] = "Admin Logged In";

                if(!string.IsNullOrEmpty(removeclaimcode))
                {
                    sql.updateCollected(removeclaimcode);
                    ViewData["Message"] = "Code claimed";
                }

                if (string.IsNullOrEmpty(claimcode))
                {

                    ViewData["ShowValidate"] = 1;
                    

                }
                else
                {
                    int collectnumber = 0;
                    ViewData["Message"] = claimcode;
                    collectnumber = sql.badgesQR(claimcode);
                    ViewData["CollectNumber"] = collectnumber;
                    if ( collectnumber > 0 )
                    {
                        ViewData["ShowClaim"] = 1;
                        ViewData["ClaimCode"] = claimcode;
                    }
                    else
                    {
                        ViewData["Message"] = "Invalid or used Code";
                        ViewData["ShowValidate"] = 1;
                    }
                    
                }



            }


            return View();
        }
    }
}
