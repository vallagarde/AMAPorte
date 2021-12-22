using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
using Projet2.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2.Controllers
{
    public class PaymentController : Controller
    {
        // GET: /<controller>/
        public IActionResult Paiement()
        {
            HomeViewModel hvm = new HomeViewModel
            {

                PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId")

            };
            return View(hvm);
        }

        [HttpPost]
        public IActionResult Paiement(int a)
        {
            HomeViewModel hvm = new HomeViewModel
            {

                PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId")

            };
            return View(hvm);
        }
    }
}
