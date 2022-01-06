using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projet2.Helpers;
using Projet2.Models;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using Projet2.Models.PanierSaisonniers;
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

        public IActionResult Activite()
        {
            return View();
        }

        public IActionResult Index()
        {
            CompteServices csx = new CompteServices();
            HomeViewModel hvm = new HomeViewModel
            {
                PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId"),
                ListeComptesAdP = csx.ObtenirAdPsVedettes()

        };
            return View(hvm);
        }

        public IActionResult Privacy()
        {
            HomeViewModel hvm = new HomeViewModel
            {
                PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId")

            };
            return View(hvm);
        }
        public IActionResult Producteurs()
        {
            HomeViewModel hvm = new HomeViewModel();
            ArticleRessources articleRessources = new ArticleRessources();
            PanierSaisonnierService saisonnierService = new PanierSaisonnierService();
            CompteServices cs = new CompteServices();

            hvm.ListeComptesAdP = cs.ObtenirTousLesAdPs();
            foreach (AdP adp in hvm.ListeComptesAdP)
            {
                if (adp.EstActive)
                {
                    foreach (Article article in articleRessources.ObtenirArticleParAdP(adp))
                    {
                        if (!article.EstValide)
                        {
                            hvm.AdP.Assortiment.Add(article);
                        }
                    }
                    foreach (PanierSaisonnier panierSaisonnier in saisonnierService.ObtenirPanierParAdP(adp))
                    {
                        if (!panierSaisonnier.EstValide)
                        {
                            hvm.AdP.AssortimentPanier.Add(panierSaisonnier);
                        }
                    }

                }
                
            }

            return View(hvm);
        }

        public IActionResult Producteur(int Id)
        {
            HomeViewModel hvm = new HomeViewModel();
            ArticleRessources articleRessources = new ArticleRessources();
            PanierSaisonnierService saisonnierService = new PanierSaisonnierService();
            CompteServices cs = new CompteServices();

            hvm.AdP = cs.ObtenirAdPParId(Id);

            //foreach (Article article in articleRessources.ObtenirArticleParAdP(hvm.AdP))
            //{
            //    if (!article.EstValide)
            //    {
            //        hvm.AdP.Assortiment.Add(article);
            //    }
            //}
            //foreach (PanierSaisonnier panierSaisonnier in saisonnierService.ObtenirPanierParAdP(hvm.AdP))
            //{
            //    if (!panierSaisonnier.EstValide)
            //    {
            //        hvm.AdP.AssortimentPanier.Add(panierSaisonnier);
            //    }
            //}
            return View(hvm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
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

        [Authorize]
        public IActionResult CommandesUser()
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
                    return RedirectToAction("CommandesAPreparer", "EspaceAdP", hvm.AdP);
                }
                else if (viewModel.Identifiant.EstAdA == true)
                {
                    hvm.AdA = cs.ObtenirAdAParIdentifiant(viewModel.Identifiant.Id);
                    return RedirectToAction("Commandes", "CompteAdA", hvm.AdA);
                }
                else if (viewModel.Identifiant.EstCE == true)
                {
                    hvm.ContactComiteEntreprise = cs.ObtenirCCEParIdentifiant(viewModel.Identifiant.Id);
                    return RedirectToAction("Commandes", "CompteCE", hvm.ContactComiteEntreprise);
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
