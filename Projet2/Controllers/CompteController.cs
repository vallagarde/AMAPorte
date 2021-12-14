using Microsoft.AspNetCore.Mvc;

namespace Projet2.Controllers
{
    public class CompteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
