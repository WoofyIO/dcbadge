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
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

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
