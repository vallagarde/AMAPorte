using Microsoft.AspNetCore.Mvc;

namespace Projet2.Controllers
{
    public class AdminController : Controller
    {

        //TOUS LES VUES LIE A GESTION : GCRA; GCCQ; DSI
        public IActionResult Index()
        {
            return View();
        }
    }
}
