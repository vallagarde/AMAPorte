using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Projet2.Models.Compte;
using Projet2.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Projet2.Controllers
{
    public class CompteAdPController : Controller
    {
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
                //View("ArticlesFavoris", hvm);
                return View(hvm);
            }
        }

        [HttpGet]
        public IActionResult CreationCompte()
        {
            //ViewBag.listePaiements = Paiement.listePaiements;
            return View();
        }

        [HttpPost]
        public IActionResult CreationCompte(Personne personne, Identifiant identifiant, Adresse adresse, AdP adp)
        {

            if (personne != null && identifiant != null)
            {
                int id = cs.AjouterIdentifiant(identifiant.AdresseMail, identifiant.MotDePasse);

                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, id.ToString()),
                };

                var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });
                HttpContext.SignInAsync(userPrincipal);

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
            hvm.Personne = cs.ObtenirPersonne(adp.PersonneId);
            hvm.Adresse = cs.ObtenirAdresse(hvm.Personne.AdresseId);
            hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);
            hvm.AdP = adp;
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

            return View("Index", hvm);
        }

        public IActionResult SuppressionCompte(AdP adp)
        {
            cs.SupprimerAdP(adp.Id);
            HttpContext.SignOutAsync();
            return View();
        }

    }
}
