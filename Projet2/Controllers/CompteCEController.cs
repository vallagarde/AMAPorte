using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projet2.Models.Compte;
using Projet2.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Projet2.Controllers
{
    [Authorize]
    public class CompteCEController : Controller
    {
        //View pour paiement 12€/6 mois 
        //CRUD Compte CE ok, regarder pourquoi la liste des autres CE ne s'affiche pas directement, ajouter quelques attributs (paiement, adresse facturation), warning avant suppression compte
        //adresse avec base de données ? 
        //ajouter foncionnalité d'ajouter des produits, panier xNombreUtilisateur dans la boutique 
        //ajouter foncionnalité favoriser dans la boutique pour les utilisateurs connectés
        //mettre les informations de son compte dans une vue "Mon Compte" 
        //BOUTIQUE / PANIERS S. : 
        //avoir access a l'onglet producteur pour en reserver des paniers/favoriser des producteurs
        //voir ses paniers s./commandes en boutique en cours dans une vue
        //voir l'historique de ses commandes panier s. et boutique, ajouter un seul ! avis par panier ou article
        //ATELIERS :
        //avoir access a l'onglet ateliers pour en reserver/favoriser
        //voir les ateliers a venir sur sa page d'accueil 
        //voir l'historique des ateliers passés, ajouter un seul ! avis par atelier

        CompteServices cs = new CompteServices();
        HomeViewModel hvm = new HomeViewModel();
        public IActionResult Index(ContactComiteEntreprise contactComiteEntreprise)
        {

            if (contactComiteEntreprise.Id == 0)
            {
                return View("Error");
            }
            else
            {
                hvm.Entreprise = cs.ObtenirEntreprise(contactComiteEntreprise.EntrepriseId);
                hvm.Entreprise.ListeContact = cs.ObtenirCCEsParEntreprise(hvm.Entreprise.Id);
                hvm.Adresse = cs.ObtenirAdresse(hvm.Entreprise.AdresseId);
                hvm.Identifiant = cs.ObtenirIdentifiant(contactComiteEntreprise.IdentifiantId);
                hvm.ContactComiteEntreprise = contactComiteEntreprise;
                return View(hvm);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CreationCompte()
        {
            //ViewBag.listePaiements = Paiement.listePaiements;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreationCompte(ContactComiteEntreprise contactComiteEntreprise,Entreprise entreprise, Identifiant identifiant, Adresse adresse)
        {

            if (contactComiteEntreprise != null && identifiant != null && entreprise != null)
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

                
                contactComiteEntreprise.IdentifiantId = id;
                contactComiteEntreprise.AdresseMail = identifiant.AdresseMail;

                hvm.ContactComiteEntreprise = cs.CreerCCE(contactComiteEntreprise, entreprise, adresse);
                hvm.Entreprise = entreprise;
                hvm.Entreprise.ListeContact = cs.ObtenirCCEsParEntreprise(hvm.Entreprise.Id);
                hvm.Adresse = adresse;
                hvm.Identifiant = identifiant;

                return View("Index", hvm);
            }
            return View();

        }

        public IActionResult ArticlesFavoris(ContactComiteEntreprise contactComiteEntreprise)
        {
            hvm.Entreprise = cs.ObtenirEntreprise(contactComiteEntreprise.EntrepriseId);
            hvm.ContactComiteEntreprise = contactComiteEntreprise;
            hvm.Entreprise.ListeContact = cs.ObtenirCCEsParEntreprise(hvm.Entreprise.Id);
            return View(hvm);
        }

        public IActionResult AteliersFavoris(ContactComiteEntreprise contactComiteEntreprise)
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
            hvm.Identifiant = cs.ModifierIdentifiant(identifiant);
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

        public IActionResult SuppressionCompte(ContactComiteEntreprise contactComiteEntreprise)
        {
            List<ContactComiteEntreprise> listCCE = cs.ObtenirCCEsParEntreprise(contactComiteEntreprise.EntrepriseId);

            if (listCCE.Count != 1)
            {
                cs.SupprimerCCE(contactComiteEntreprise.Id);
                HttpContext.SignOutAsync();
                return View();
                
            } else
            {
                cs.SupprimerEntreprise(contactComiteEntreprise.EntrepriseId);
                HttpContext.SignOutAsync();
            }

            return View();
        }

    }
}
