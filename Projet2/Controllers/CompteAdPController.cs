using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
using Projet2.Models.Compte;
using Projet2.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Projet2.Controllers
{
    [Authorize]
    public class CompteAdPController : Controller
    {
        //ajouter quelques attributs (RIB, photo), warning avant suppression compte
        //adresse avec base de données ? 
        //mettre les informations de son compte dans une vue "Mon Compte" 
       
        CompteServices cs = new CompteServices();
        HomeViewModel hvm = new HomeViewModel();
        public IActionResult Index(AdP adp)
        {
            hvm.Personne = cs.ObtenirPersonne(adp.PersonneId);
            hvm.Adresse = cs.ObtenirAdresse(hvm.Personne.AdresseId);
            hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);
            hvm.AdP = adp;
            if (hvm.Personne == null)
            {
                return View("Error");
            }
            else
            {
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
        public IActionResult CreationCompte(Personne personne, Identifiant identifiant, Adresse adresse, AdP adp)
        {

            if (personne != null && identifiant != null)
            {
                adp.EstAdP = true;
                identifiant.EstAdP = adp.EstAdP;
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


                personne.IdentifiantId = id;

                hvm.AdP = cs.CreerAdP(personne, adresse, adp);

                hvm.Personne = personne;
                hvm.Adresse = adresse;
                hvm.Identifiant = identifiant;
                hvm.AdP.PersonneId = hvm.Personne.Id;

                return View("Index", hvm);
            }
            return View();

        }

        [HttpGet]
        public IActionResult ModificationCompte(AdP adp)
        {
            hvm.AdP = cs.ObtenirAdPParId(adp.Id);
            hvm.Personne = cs.ObtenirPersonne(hvm.AdP.PersonneId);
            hvm.Adresse = cs.ObtenirAdresse(hvm.Personne.AdresseId);
            hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);

            return View(hvm);

        }

        [HttpPost]
        public IActionResult ModificationCompte(Personne personne, Identifiant identifiant, Adresse adresse, AdP adp)
        {

            hvm.Adresse = cs.ModifierAdresse(adresse);
            hvm.Identifiant = cs.ModifierIdentifiant(identifiant);
            personne.AdresseId = hvm.Adresse.Id;
            personne.IdentifiantId = hvm.Identifiant.Id;
            hvm.Personne = cs.ModifierPersonne(personne);
            adp.PersonneId = hvm.Personne.Id;
            hvm.AdP = cs.ModifierAdP(adp);

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            if (viewModel.Authentifie)
            {
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdP == true)
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

        public IActionResult SuppressionCompte(AdP adp)
        {
            cs.SupprimerAdP(adp.Id);
            HttpContext.SignOutAsync();
            return View();
        }



    }
}
