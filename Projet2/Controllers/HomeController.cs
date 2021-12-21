using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projet2.Helpers;
using Projet2.Models;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;
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

        public IActionResult Inscription()
        {
            return View();
        }

        public IActionResult EspacePersonnel()
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                HomeViewModel hvm = new HomeViewModel();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdP == true)
                {
                    hvm.AdP = cs.ObtenirAdPParIdentifiant(viewModel.Identifiant.Id);
                    return RedirectToAction("Index", "CompteAdP", hvm.AdP);
                }
                else if (viewModel.Identifiant.EstAdA == true)
                {
                    hvm.AdA = cs.ObtenirAdAParIdentifiant(viewModel.Identifiant.Id);
                    return RedirectToAction("Index", "CompteAdA", hvm.AdA);
                }
                else if (viewModel.Identifiant.EstCE == true)
                {
                    hvm.ContactComiteEntreprise = cs.ObtenirCCEParIdentifiant(viewModel.Identifiant.Id);
                    return RedirectToAction("Index", "CompteCE", hvm.ContactComiteEntreprise);
                }
                else if ((viewModel.Identifiant.EstGCCQ == true) || (viewModel.Identifiant.EstGCRA == true) || (viewModel.Identifiant.EstDSI == true))
                {
                    hvm.Identifiant = viewModel.Identifiant;
                    hvm.Admin = cs.ObtenirAdminParIdentifiant(viewModel.Identifiant.Id);
                    return RedirectToAction("Index", "Admin", hvm.Admin);
                }
            }
            return RedirectToAction("Index", "Login");
        }

    }
}
