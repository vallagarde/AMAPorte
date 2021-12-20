using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Projet2.Models.Compte;
using Projet2.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Projet2.Controllers
{
    public class CompteCEController : Controller
    {
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

        [HttpGet]
        public IActionResult CreationCompte()
        {
            //ViewBag.listePaiements = Paiement.listePaiements;
            return View();
        }

        [HttpPost]
        public IActionResult CreationCompte(ContactComiteEntreprise contactComiteEntreprise,Entreprise entreprise, Identifiant identifiant, Adresse adresse)
        {

            if (contactComiteEntreprise != null && identifiant != null && entreprise != null)
            {
                int id = cs.AjouterIdentifiant(identifiant.AdresseMail, identifiant.MotDePasse);

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
            hvm.Entreprise = cs.ObtenirEntreprise(contactComiteEntreprise.EntrepriseId);
            hvm.Adresse = cs.ObtenirAdresse(hvm.Entreprise.AdresseId);
            hvm.Identifiant = cs.ObtenirIdentifiant(contactComiteEntreprise.IdentifiantId);
            hvm.ContactComiteEntreprise = contactComiteEntreprise;
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

            return View("Index", hvm);
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
