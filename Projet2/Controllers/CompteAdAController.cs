using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Projet2.Models.Compte;
using Projet2.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Projet2.Controllers
{
    public class CompteAdAController : Controller
    {
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

        [HttpGet]
        public IActionResult CreationCompte()
        {
            //ViewBag.listePaiements = Paiement.listePaiements;
            return View();
        }

        [HttpPost]
        public IActionResult CreationCompte(Personne personne, Identifiant identifiant, Adresse adresse)
        {

            if (personne != null && identifiant != null && adresse != null)
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

                hvm.AdA = cs.CreerAdA(personne, identifiant, adresse);
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
            hvm.Personne = cs.ObtenirPersonne(ada.PersonneId);
            hvm.Adresse = cs.ObtenirAdresse(hvm.Personne.AdresseId);
            hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);
            hvm.AdA = ada;
            return View(hvm);
        }

        [HttpPost]
        public IActionResult ModificationCompte(Personne personne, Identifiant identifiant, Adresse adresse, AdA ada)
        {

            hvm.AdA = cs.ModifierAdA(ada);
            hvm.Personne = cs.ModifierPersonne(cs.ObtenirPersonne(hvm.AdA.PersonneId));
            hvm.Adresse = cs.ModifierAdresse(cs.ObtenirAdresse(hvm.Personne.Adresse.Id));
            hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);
            return View("Index", hvm);
        }

        public IActionResult SuppressionCompte(AdA ada)
        {
            cs.SupprimerAdA(ada.Id);
            return View();
        }

    }
}
