using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using Projet2.Models.PanierSaisonniers;
using Projet2.ViewModels;
using System.Linq;

namespace Projet2.Controllers
{
    [Authorize]
    public class EspaceAdPController : Controller
    {
        //BOUTIQUE
        //OK\\ ajouter des articles à la boutique, les modifier,  les supprimer, les afficher dans son espace personnel
        //!\\voir les KPI sur les ventes, voir les commandes (historique + en cours(a preparer, a livrer))
        //PANIER
        //!\\ajouter des paniers s., les modifier, les supprimer (qu'avant une date specifique prennant en compte les commandes en amont) les afficher dans son espace personnel,
        //!\\voir les KPI sur les ventes, voir les commandes (historique + en cours(a preparer, a livrer))
        //ATELIER
        //!\\ajouter des ateliers, les modifier, les annuler, les afficher dans son espace personnel
        //!\\(avec informations sur les participants (nom, prenom, telephone, /nom entreprise et nombre participants pour CE))

        private CompteServices cs = new CompteServices();
        private ArticleRessources ar = new ArticleRessources();
        private PanierSaisonnierService pss = new PanierSaisonnierService();
        private HomeViewModel hvm = new HomeViewModel();
        public IActionResult Index()
        {
            return View();
        }


        //LIAISONS BOUTIQUE
        public IActionResult GestionBoutique(AdP adp)
        {
            ar.ObtenirArticleParAdP(adp);
            hvm.AdP = adp;
            return View(hvm);
        }

        [HttpGet]
        public IActionResult ArticleModification(Article article)
        {
            hvm.Article = article;
            return View(hvm);
        }
        [HttpPost]
        public IActionResult ArticleModification(Article article, AdP adp)
        {
            ar.ModifierArticle(article.Id, article.Nom, article.Description, article.Prix, article.Stock, article.PrixTTC, adp.Id);

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdP == true)
                {
                    HomeViewModel hvm = new HomeViewModel();
                    hvm.AdP = cs.ObtenirAdPParIdentifiant(viewModel.Identifiant.Id);
                    hvm.Article.Id = article.Id;
                    return RedirectToAction("GestionBoutique", "EspaceAdP", hvm.AdP);
                }
            }
            return RedirectToAction("Index", "Login");

        }
        public IActionResult SupprimerArticle(Article article)
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdP == true)
                {
                    AdP adp = cs.ObtenirAdPParIdentifiant(viewModel.Identifiant.Id);
                    ar.SupprimerArticle(article.Id);
                    return RedirectToAction("GestionBoutique", "EspaceAdP", adp);
                }
                return RedirectToAction("EspacePersonnel", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        //LIAISONS PANIER
        public IActionResult GestionPanier(AdP adp)
        {
            pss.ObtenirPanierParAdP(adp);
            hvm.AdP = adp;
            return View(hvm);
        }

        [HttpGet]
        public IActionResult PanierModification(PanierSaisonnier panierSaisonnier)
        {
            hvm.PanierSaisonnier = panierSaisonnier;
            return View(hvm);
        }

        [HttpPost]
        public IActionResult PanierModification(PanierSaisonnier panierSaisonnier, AdP adp)
        {
            //panierSaisonnier.AdP = adp;
            pss.ModifierPanierSaisonnier(panierSaisonnier);

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdP == true)
                {
                    HomeViewModel hvm = new HomeViewModel();
                    hvm.AdP = cs.ObtenirAdPParIdentifiant(viewModel.Identifiant.Id);
                    hvm.PanierSaisonnier.Id = panierSaisonnier.Id;
                    return RedirectToAction("GestionPanier", "EspaceAdP", hvm.AdP);
                }
            }
            return RedirectToAction("Index", "Login");

        }

        public IActionResult SupprimerPanier(PanierSaisonnier panierSaisonnier)
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdP == true)
                {
                    AdP adp = cs.ObtenirAdPParIdentifiant(viewModel.Identifiant.Id);
                    pss.SupprimerPanierSaisonnier(panierSaisonnier.Id);
                    return RedirectToAction("GestionPanier", "EspaceAdP", adp);
                }
                return RedirectToAction("EspacePersonnel", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

    }
}
