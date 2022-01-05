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
    public class CompteCEController : Controller
    {
        //ajouter foncionnalité favoriser dans la boutique pour les utilisateurs connectés
        //ATELIERS :
        //avoir access a l'onglet ateliers pour en reserver/favoriser
        //voir les ateliers a venir sur sa page d'accueil 
        //voir l'historique des ateliers passés, ajouter un seul ! avis par atelier

        CompteServices cs = new CompteServices();
        PanierService panierService = new PanierService();
        LignePanierService lignePanierService = new LignePanierService();
        HomeViewModel hvm = new HomeViewModel();
        public IActionResult Index(ContactComiteEntreprise contactComiteEntreprise)
        {

            if (contactComiteEntreprise.Id == 0)
            {
                UtilisateurViewModel viewModel = new UtilisateurViewModel() { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                hvm.ContactComiteEntreprise = cs.ObtenirCCEParIdentifiant(viewModel.Identifiant.Id);
                if (hvm.ContactComiteEntreprise == null)
                {
                    return View("Error");
                }
                else
                {
                    hvm.Entreprise = cs.ObtenirEntreprise(hvm.ContactComiteEntreprise.EntrepriseId);
                    hvm.Entreprise.ListeContact = cs.ObtenirCCEsParEntreprise(hvm.Entreprise.Id);
                    hvm.Adresse = cs.ObtenirAdresse(hvm.Entreprise.AdresseId);
                    hvm.Identifiant = cs.ObtenirIdentifiant(hvm.ContactComiteEntreprise.IdentifiantId);
                    return View(hvm);
                }
            }
            else
            {
                hvm.ContactComiteEntreprise = contactComiteEntreprise;
                hvm.Entreprise = cs.ObtenirEntreprise(hvm.ContactComiteEntreprise.EntrepriseId);
                hvm.Entreprise.ListeContact = cs.ObtenirCCEsParEntreprise(hvm.Entreprise.Id);
                hvm.Adresse = cs.ObtenirAdresse(hvm.Entreprise.AdresseId);
                hvm.Identifiant = cs.ObtenirIdentifiant(hvm.ContactComiteEntreprise.IdentifiantId);
                return View(hvm);
            }
        }

        public IActionResult CECompteInfo(ContactComiteEntreprise contactComiteEntreprise)
        {

            if (contactComiteEntreprise.Id == 0)
            {
                return View("Error");
            }
            else
            {
                hvm.ContactComiteEntreprise = contactComiteEntreprise;
                hvm.Entreprise = cs.ObtenirEntreprise(hvm.ContactComiteEntreprise.EntrepriseId);
                hvm.Entreprise.ListeContact = cs.ObtenirCCEsParEntreprise(hvm.Entreprise.Id);
                hvm.Adresse = cs.ObtenirAdresse(hvm.Entreprise.AdresseId);
                hvm.Identifiant = cs.ObtenirIdentifiant(hvm.ContactComiteEntreprise.IdentifiantId);
                return View(hvm);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CreationCompte()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreationCompte(ContactComiteEntreprise contactComiteEntreprise,Entreprise entreprise, Identifiant identifiant, Adresse adresse)
        {
            if (!cs.TrouverIdentifiant(identifiant))
            {
                //adresse avec base de données ? 
                if (contactComiteEntreprise != null && identifiant != null && entreprise != null)
                {
                    if (entreprise.EstEnAccord == true)
                    {
                        contactComiteEntreprise.EstCE = true;
                        identifiant.EstCE = contactComiteEntreprise.EstCE;
                        int id = cs.AjouterIdentifiant(identifiant);

                        var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, id.ToString()),
                };

                        var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

                        var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });
                        HttpContext.SignInAsync(userPrincipal);
                        bool EstUtilisateur = true;
                        SessionHelper.SetObjectAsJson(HttpContext.Session, "authentification", EstUtilisateur);

                        contactComiteEntreprise.IdentifiantId = id;
                        contactComiteEntreprise.AdresseMail = identifiant.AdresseMail;

                        hvm.ContactComiteEntreprise = cs.CreerCCE(contactComiteEntreprise, entreprise, adresse);
                        hvm.Entreprise = cs.ObtenirEntreprise(hvm.ContactComiteEntreprise.EntrepriseId);
                        hvm.Entreprise.ListeContact = cs.ObtenirCCEsParEntreprise(hvm.Entreprise.Id);
                        hvm.Adresse = cs.ObtenirAdresse(hvm.Entreprise.AdresseId);
                        hvm.Identifiant = cs.ObtenirIdentifiant(hvm.ContactComiteEntreprise.IdentifiantId);

                        return View("Index", hvm);
                    }
                }
            }
            hvm.AdresseExistante = cs.TrouverIdentifiant(identifiant);
            return View(hvm);
        }

        public IActionResult Commandes(ContactComiteEntreprise contactComiteEntreprise)
        {
            hvm.Entreprise = cs.ObtenirEntreprise(contactComiteEntreprise.EntrepriseId);
            hvm.Entreprise.CommandesBoutiqueEffectues = panierService.ObtenirCommandesParEntreprise(hvm.Entreprise);
            hvm.Entreprise.CommandesPanierEffectues = lignePanierService.ObtenirCommandesPanierParEntreprise(hvm.Entreprise);
            hvm.ContactComiteEntreprise = contactComiteEntreprise;
            return View(hvm);
        }

        public IActionResult HistoriqueCommandes(ContactComiteEntreprise contactComiteEntreprise)
        {
            hvm.Entreprise = cs.ObtenirEntreprise(contactComiteEntreprise.EntrepriseId);
            hvm.Entreprise.CommandesBoutiqueEffectues = panierService.ObtenirCommandesParEntreprise(hvm.Entreprise);
            hvm.Entreprise.CommandesPanierEffectues = lignePanierService.ObtenirCommandesPanierParEntreprise(hvm.Entreprise);
            hvm.ContactComiteEntreprise = contactComiteEntreprise;
            return View(hvm);
        }
        


        [HttpGet]
        public IActionResult ModificationCompte(ContactComiteEntreprise contactComiteEntreprise)
        {
            hvm.ContactComiteEntreprise = cs.ObtenirCCEParId(contactComiteEntreprise.Id);
            hvm.Entreprise = cs.ObtenirEntreprise(hvm.ContactComiteEntreprise.EntrepriseId);
            hvm.Adresse = cs.ObtenirAdresse(hvm.Entreprise.AdresseId);
            hvm.Identifiant = cs.ObtenirIdentifiant(hvm.ContactComiteEntreprise.IdentifiantId);
            hvm.Entreprise.ListeContact = cs.ObtenirCCEsParEntreprise(hvm.Entreprise.Id);
            return View(hvm);
        }

        [HttpPost]
        public IActionResult ModificationCompte(ContactComiteEntreprise contactComiteEntreprise, Entreprise entreprise, Identifiant identifiant, Adresse adresse)
        {

            hvm.Adresse = cs.ModifierAdresse(adresse);
            entreprise.AdresseId = hvm.Adresse.Id;
            contactComiteEntreprise.IdentifiantId = hvm.Identifiant.Id;
            contactComiteEntreprise.AdresseMail = hvm.Identifiant.AdresseMail;
            hvm.Entreprise = cs.ModifierEntreprise(entreprise);
            contactComiteEntreprise.EntrepriseId = hvm.Entreprise.Id;
            hvm.ContactComiteEntreprise = cs.ModifierCCE(contactComiteEntreprise);
            hvm.Entreprise.ListeContact = cs.ObtenirCCEsParEntreprise(hvm.Entreprise.Id);

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            if (viewModel.Authentifie)
            {
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstCE == true)
                {
                    return View("Index", hvm);
                }
                else if ((viewModel.Identifiant.EstGCCQ == true) || (viewModel.Identifiant.EstGCRA == true) || (viewModel.Identifiant.EstDSI == true))
                {
                    HomeViewModel hvmAdmin = new HomeViewModel();
                    hvmAdmin.Identifiant = viewModel.Identifiant;
                    hvmAdmin.Admin = cs.ObtenirAdminParIdentifiant(viewModel.Identifiant.Id);
                    return RedirectToAction("GestionComptes", "Admin", hvmAdmin.Admin);
                }
                return View(hvm);
            }
            return Redirect("/");
        }


        public IActionResult SuppressionCompte(ContactComiteEntreprise contactComiteEntreprise, int Id)
        {
            List<ContactComiteEntreprise> listCCE = cs.ObtenirCCEsParEntreprise(contactComiteEntreprise.EntrepriseId);

            if (listCCE.Count != 1)
            {
                cs.SupprimerCCE(contactComiteEntreprise.Id);
                bool EstUtilisateur = false;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "authentification", EstUtilisateur);
                HttpContext.SignOutAsync();
                return Redirect("/");

            }
            else
            {
                cs.SupprimerEntreprise(contactComiteEntreprise.EntrepriseId);
                bool EstUtilisateur = false;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "authentification", EstUtilisateur);
                HttpContext.SignOutAsync();
            }

            return Redirect("/");
        }


        public IActionResult ArticlesFavoris(ContactComiteEntreprise contactComiteEntreprise)
        {
            hvm.Entreprise = cs.ObtenirEntreprise(contactComiteEntreprise.EntrepriseId);
            hvm.ContactComiteEntreprise = contactComiteEntreprise;
            hvm.Entreprise.ListeContact = cs.ObtenirCCEsParEntreprise(hvm.Entreprise.Id);
            return View(hvm);
        }

        public IActionResult ProducteursFavoris(ContactComiteEntreprise contactComiteEntreprise)
        {
            hvm.Entreprise = cs.ObtenirEntreprise(contactComiteEntreprise.EntrepriseId);
            hvm.ContactComiteEntreprise = contactComiteEntreprise;
            hvm.Entreprise.ListeContact = cs.ObtenirCCEsParEntreprise(hvm.Entreprise.Id);
            return View(hvm);
        }

        //voir l'historique des ateliers passés, ajouter un seul ! avis par atelier
        public IActionResult HistoriqueAteliers(ContactComiteEntreprise contactComiteEntreprise)
        {
            return View(hvm);
        }
        public IActionResult AteliersFavoris(ContactComiteEntreprise contactComiteEntreprise)
        {
            hvm.Entreprise = cs.ObtenirEntreprise(contactComiteEntreprise.EntrepriseId);
            hvm.ContactComiteEntreprise = contactComiteEntreprise;
            hvm.Entreprise.ListeContact = cs.ObtenirCCEsParEntreprise(hvm.Entreprise.Id);
            return View(hvm);
        }


    }
}
