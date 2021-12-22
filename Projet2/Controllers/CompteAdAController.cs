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
    public class CompteAdAController : Controller
    {
        //View pour paiement 12€/6 mois 
        //CRUD Compte CE ok, regarder pourquoi la liste des autres CE ne s'affiche pas directement, ajouter quelques attributs (paiement, photo, adresse facturation), warning avant suppression compte
        //adresse avec base de données ? 
        //ajouter foncionnalité favoriser dans la boutique pour les utilisateurs connectés
        //mettre les informations de son compte dans une vue "Mon Compte" 
        //BOUTIQUE / PANIERS S. : 
        //avoir access a l'onglet producteur pour en reserver des paniers/favoriser des producteurs
        //voir ses paniers/commandes en boutique en cours dans une vue
        //voir l'historique de ses commandes panier et boutique, ajouter un seul ! avis par panier ou article
        //ATELIERS :
        //avoir access a l'onglet ateliers pour en reserver/favoriser
        //voir les ateliers a venir sur sa page d'accueil 
        //voir l'historique des ateliers passés, ajouter un seul ! avis par atelier

        CompteServices cs = new CompteServices();
        HomeViewModel hvm = new HomeViewModel();
        public IActionResult Index(AdA ada)
        {
            hvm.Personne = cs.ObtenirPersonne(ada.PersonneId);
            hvm.Adresse = cs.ObtenirAdresse(hvm.Personne.AdresseId);
            hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);
            hvm.AdA = ada;
            if (hvm.Personne == null)
            {
                return View("Error");
            }
            else
            {
                //View("ArticlesFavoris", hvm);
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
        public IActionResult CreationCompte(Personne personne, Identifiant identifiant, Adresse adresse)
        {

            if (personne != null && identifiant != null && adresse != null)
            {
                AdA ada = new AdA() { EstAdA = true };
                identifiant.EstAdA = ada.EstAdA;
                
                int id = cs.AjouterIdentifiant(identifiant);

                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, id.ToString()),
                };

                var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });
                HttpContext.SignInAsync(userPrincipal);

                personne.IdentifiantId = id;

                hvm.AdA = cs.CreerAdA(personne, adresse, ada);
                hvm.Personne = personne;
                hvm.Adresse = adresse;
                hvm.Identifiant = identifiant;

                return View("Index", hvm);
            }
            return View();

        }

        public IActionResult ArticlesFavoris(AdA ada)
        {
            hvm.Personne = cs.ObtenirPersonne(ada.PersonneId);
            hvm.AdA = ada;
            return View(hvm);        
        }

        public IActionResult AteliersFavoris(AdA ada)
        {
            hvm.Personne = cs.ObtenirPersonne(ada.PersonneId);
            hvm.AdA = ada;
            return View(hvm);
        }

        public IActionResult ProducteursFavoris(AdA ada)
        {
            hvm.Personne = cs.ObtenirPersonne(ada.PersonneId);
            hvm.AdA = ada;
            return View(hvm);
        }

        [HttpGet]
        public IActionResult ModificationCompte(AdA ada)
        {
            hvm.AdA = cs.ObtenirAdAParId(ada.Id);
            hvm.Personne = cs.ObtenirPersonne(hvm.AdA.PersonneId);
            hvm.Adresse = cs.ObtenirAdresse(hvm.Personne.AdresseId);
            hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);

            return View(hvm);
        }

        [HttpPost]
        public IActionResult ModificationCompte(Personne personne, Identifiant identifiant, Adresse adresse, AdA ada)
        {
                
                hvm.Adresse = cs.ModifierAdresse(adresse);
                hvm.Identifiant = cs.ModifierIdentifiant(identifiant);
                personne.AdresseId = hvm.Adresse.Id;
                personne.IdentifiantId = hvm.Identifiant.Id;
                hvm.Personne = cs.ModifierPersonne(personne);
                ada.PersonneId = hvm.Personne.Id;
                hvm.AdA = cs.ModifierAdA(ada);

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            if (viewModel.Authentifie)
            {
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdA == true)
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

        public IActionResult SuppressionCompte(AdA ada)
        {
            cs.SupprimerAdA(ada.Id);
            HttpContext.SignOutAsync();
            return View();
        }

    }
}
