using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using Projet2.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Projet2.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        //TOUS LES VUES LIE A GESTION : GCRA; GCCQ; DSI
        //CRUD pour le compte Admin > OK, > warning avant suppression compte a ajouter
        //Afficher tous les comptes OK > recherche dans les tableaux a implementer, ajouter des données dans le tableau 
        //Afficher et Valider (ou envoyer des retours sur) les propositions d'articles et panier des producteurs 
        //Ajouter des Articles ou Paniers ou Ateliers pour un producteur 

        CompteServices cs = new CompteServices();
        HomeViewModel hvm = new HomeViewModel();
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
                    HomeViewModel hvm = new HomeViewModel();
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
            PanierService panierService = new PanierService();
            List<Commande> commandes = panierService.ObtenirCommandes();
            
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
            PanierService panierService = new PanierService();
            panierService.ChangerEtatCommande(commande.PanierBoutiqueId, commande.EtatCommande);
            return RedirectToAction("GestionEtatCommande", hvm.Admin);
        }

        public IActionResult GestionDemandesProducteurs(Admin admin)
        {
            hvm.Admin = admin;
            return View(hvm);
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
