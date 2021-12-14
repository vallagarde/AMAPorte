using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projet2.Models;
using Projet2.Models.Boutique;
using Projet2.ViewModels;

namespace Projet2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Producteur()
        {
            ArticleRessources ctx = new ArticleRessources();
            List<Article> articles = ctx.ObtientTousLesArticles();

            HomeViewModel hvm = new HomeViewModel
            {
               
                Boutiques = new Boutiques(){ Articles = articles, NombreArticle = articles.Count},
                
            };

            return View(hvm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
