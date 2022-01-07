using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
using Projet2.Models.Boutique;
using Projet2.Models.Calendriers;
using Projet2.Models.Compte;
using Projet2.Models.PanierSaisonniers;
using Projet2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projet2.Controllers
{
    [Authorize]
    public class EspaceAdPController : Controller
    {
        //!\\voir les KPI sur les ventes, voir les commandes (historique + en cours(a preparer, a livrer))
        //A AMELIORER : Vue calendrier (commandes a preparer)

        //ATELIER
        //!\\ajouter des ateliers, les modifier, les annuler, les afficher dans son espace personnel
        //!\\(avec informations sur les participants (nom, prenom, telephone, /nom entreprise et nombre participants pour CE))

        private CompteServices cs = new CompteServices();
        private ArticleRessources ar = new ArticleRessources();
        private PanierSaisonnierService pss = new PanierSaisonnierService();
        private CalendrierService calendrier = new CalendrierService();
        private HomeViewModel hvm = new HomeViewModel();
        private PanierService panierService = new PanierService();
        private LignePanierService lignePanierService = new LignePanierService();

        //GESTION COMMANDES
        [HttpGet]
        public IActionResult CommandesAPreparer(AdP adp)
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel() { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
            hvm.AdP = cs.ObtenirAdPParIdentifiant(viewModel.Identifiant.Id);
            hvm.AdP = adp;
            hvm.ProchaineDateLivraison = calendrier.ObtenirProchaineDateDeLivraison();
            PanierService panierService = new PanierService();
            List<Commande> commandes = panierService.ObtenirCommandesAdP(hvm.AdP.Id);
            foreach (Commande commande in commandes)
            {
                if (commande.EstEnPreparation)
                {
                    commande.DateLivraison = calendrier.ObtenirDateLivraisonCommande(commande.Id);
                    if (commande.DateLivraison.Date == hvm.ProchaineDateLivraison.Date)
                    {
                        hvm.ListeCommandesEnPrep.Add(commande);
                    }                   
                }
            }
            LignePanierService lignePanierService = new LignePanierService();
            List<CommandePanier> commandesPanier = lignePanierService.ObtenirCommandesPanierAdP(hvm.AdP.Id);
            foreach (CommandePanier commandePanier in commandesPanier)
            {
                if (commandePanier.EstEnPreparation)
                {
                    commandePanier.DateLivraison = calendrier.ObtenirDatesLivraisonPanierSaisonnier(commandePanier.Id).FirstOrDefault();
                    if (commandePanier.DateLivraison.Date == hvm.ProchaineDateLivraison.Date)
                    {
                        hvm.ListeCommandesPanierEnPrep.Add(commandePanier);
                    }
                }
            }

            return View(hvm);
        }

        [HttpPost]
        public IActionResult CommandesAPreparer(AdP adp, Commande commande)
        {
            hvm.AdP = cs.ObtenirAdPParId(adp.Id);
            PanierService panierService = new PanierService();
            if(commande.Id != 0)
            {
             panierService.ChangerEtatCommande(commande.PanierBoutiqueId, commande.EtatCommande);
            }
            return RedirectToAction("CommandesAPreparer", hvm.AdP);
        }
        

        public IActionResult Commande_infos(int CommandeId)
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel() { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
            hvm.AdP = cs.ObtenirAdPParIdentifiant(viewModel.Identifiant.Id);
            hvm.Commande = panierService.ObtenirCommandesAdP(hvm.AdP.Id).Where(c => c.Id == CommandeId).FirstOrDefault();
            return View(hvm);
        }

        public IActionResult CommandePanier_infos(int commandePanierId)
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel() { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
            hvm.AdP = cs.ObtenirAdPParIdentifiant(viewModel.Identifiant.Id);
            hvm.CommandePanier = lignePanierService.ObtenirCommandesPanierAdP(hvm.AdP.Id).Where(c => c.Id == commandePanierId).FirstOrDefault();

            return View(hvm);
        }

        [HttpPost]
        public IActionResult CommandesPanierAPreparer(AdP adp, CommandePanier commandePanier)
        {
            hvm.AdP = cs.ObtenirAdPParId(adp.Id);
            LignePanierService lignePanierService = new LignePanierService();
            if (commandePanier.Id != 0)
            {
                lignePanierService.ChangerEtatCommandePanier(commandePanier.Id, commandePanier.EtatCommande);
            }
            return RedirectToAction("CommandesAPreparer", hvm.AdP);
        }

        public IActionResult HistoriqueBoutique(AdP adp)
        {
            hvm.AdP = adp;
            PanierService panierService = new PanierService();
            List<Commande> commandes = panierService.ObtenirCommandesAdP(adp.Id);
            foreach (Commande commande in commandes)
            {
                if (commande.EstEnLivraison || commande.EstARecuperer || commande.EstLivre)
                {
                    hvm.ListeCommandesLivres.Add(commande);
                }
            }
            return View(hvm);
        }

        public IActionResult HistoriquePaniers(AdP adp)
        {
            hvm.AdP = adp;
            LignePanierService lignePanierService = new LignePanierService();
            List<CommandePanier> commandesPanier = lignePanierService.ObtenirCommandesPanierAdP(adp.Id);
            foreach (CommandePanier commandePanier in commandesPanier)
            {
                if (commandePanier.EstEnLivraison || commandePanier.EstARecuperer || commandePanier.EstLivre)
                {
                    hvm.ListeCommandesPanierLivres.Add(commandePanier);
                }
            }
            return View(hvm);
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
        public IActionResult ArticleModification(Article article, int Id)
        {
            ar.ModifierArticle(article.Id, article.Nom, article.Description, article.Prix, article.Stock, article.PrixTTC, article.AdPId);

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
