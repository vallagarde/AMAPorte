using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using Projet2.Models.PanierSaisonniers;
using Projet2.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Projet2.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        //TOUS LES VUES LIE A GESTION : GCRA; GCCQ; DSI
        //Afficher tous les comptes OK > recherche dans les tableaux a implementer, ajouter des données dans le tableau 
        //Afficher et Valider (ou envoyer des retours sur) les propositions d'articles et panier des producteurs 
        //Ajouter des Articles ou Paniers ou Ateliers pour un producteur 

        private CompteServices cs = new CompteServices();
        private HomeViewModel hvm = new HomeViewModel();
        private PanierService panierService = new PanierService();
        private ArticleRessources articleRessources = new ArticleRessources();
        public IActionResult Index(Admin admin)
        {
            hvm.Identifiant = cs.ObtenirIdentifiant(admin.IdentifiantId);
            hvm.Admin = cs.ObtenirAdminParIdentifiant(admin.IdentifiantId);
            if (hvm.Identifiant == null)
            {
                return View("Error");
            }
            else
            {
                //View("ArticlesFavoris", hvm);
                return View(hvm);
            }
        }

        [HttpGet]
        public IActionResult CreationCompte(Admin admin)
        {
            hvm.Admin = admin;
            return View(hvm);
        }

        [HttpPost]
        public IActionResult CreationCompte(Admin admin, Identifiant identifiant)
        {
            if (!cs.TrouverIdentifiant(identifiant))
            {
                if (admin != null && identifiant != null)
                {
                    if (admin.EstDSI == true)
                    {
                        identifiant.EstDSI = true;
                    }
                    else if (admin.EstGCRA == true)
                    {
                        identifiant.EstGCRA = true;
                    }
                    else
                    {
                        identifiant.EstGCCQ = true;
                    }

                    int id = cs.AjouterIdentifiant(identifiant);
                    admin.IdentifiantId = id;
                    cs.CreerAdmin(admin);

                    UtilisateurViewModel viewModel = new UtilisateurViewModel();
                    viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                    hvm.Admin = cs.ObtenirAdminParIdentifiant(viewModel.Identifiant.Id);
                    return View("Index", hvm);
                }
            }
            hvm.Admin.IdentifiantId = admin.IdentifiantId;
            hvm.AdresseExistante = cs.TrouverIdentifiant(identifiant);
            return View(hvm);
        }

        [HttpGet]
        public IActionResult GestionEtatCommande(Admin admin)
        {
            hvm.Admin = admin;
            
            List<Commande> commandes = panierService.ObtenirToutesCommandes();
            
            foreach (Commande commande in commandes)
            {
                if (commande.EstEnPreparation)
                {
                    hvm.ListeCommandesEnPrep.Add(commande);
                }
                else if (commande.EstEnLivraison)
                {
                    hvm.ListeCommandesEnCours.Add(commande);
                }
                else if (commande.EstARecuperer)
                {
                    hvm.ListeCommandesARecup.Add(commande);
                }
                else hvm.ListeCommandesLivres.Add(commande);
            }
            return View(hvm);
        }

        [HttpPost]
        public IActionResult GestionEtatCommande(Admin admin, Commande commande)
        {
            hvm.Admin = admin;
            panierService.ChangerEtatCommande(commande.PanierBoutiqueId, commande.EtatCommande);
            return RedirectToAction("GestionEtatCommande", hvm.Admin);
        }

        [HttpGet]
        public IActionResult GestionDemandesProducteurs(Admin admin)
        {
            hvm.ListeComptesAdP = cs.ObtenirTousLesAdPs();
            foreach (AdP adp in hvm.ListeComptesAdP)
            {                         
              foreach(Article article in articleRessources.ObtenirArticleParAdP(adp))
                {
                    if (!article.EstValide)
                    {
                        hvm.AdP.Assortiment.Add(article);
                    }
                }
            }
            hvm.Admin = admin;
            return View(hvm);
        }

        [HttpPost]
        public IActionResult GestionDemandesProducteurs(Admin admin, Article article, PanierSaisonnier panierSaisonnier)
        {
            if(article != null)
            {
                articleRessources.ValidationArticle(article);
                //message à producteur ?
            }
            if (panierSaisonnier != null)
            {
                //Fonction dans panier services
                articleRessources.ValidationArticle(article);
            }

            hvm.Admin = admin;
            return RedirectToAction("GestionDemandesProducteurs", hvm.Admin);
        }

        public IActionResult GestionComptes(Admin admin)
        {
            hvm.ListeComptesAdA = cs.ObtenirTousLesAdAs();
            hvm.ListeComptesAdP = cs.ObtenirTousLesAdPs();
            hvm.ListeComptesCCEs = cs.ObtenirTousLesCCEs();
            hvm.Admin = admin;
            return View(hvm);
        }

        [HttpGet]
        public IActionResult ModificationCompte(Admin admin)
        {
            hvm.Identifiant = cs.ObtenirIdentifiant(admin.IdentifiantId);
            hvm.Admin = admin;
            return View(hvm);
        }

        [HttpPost]
        public IActionResult ModificationCompte(Admin admin, Identifiant identifiant)
        {

            hvm.Identifiant = cs.ModifierIdentifiant(identifiant);
            admin.IdentifiantId = hvm.Identifiant.Id;
            hvm.Admin = cs.ModifierAdmin(admin);
            return View("Index", hvm);
        }

        public IActionResult SuppressionCompte(Admin admin)
        {
            cs.SupprimerAdmin(admin.Id);
            bool EstUtilisateur = false;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "authentification", EstUtilisateur);
            HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}
