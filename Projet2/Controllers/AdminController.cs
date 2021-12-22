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
            hvm.Admin = admin;
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CreationCompte()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreationCompte(Admin admin, Identifiant identifiant)
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

                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, id.ToString()),
                };

                var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });
                HttpContext.SignInAsync(userPrincipal);

                admin.IdentifiantId = id;

                hvm.Admin = cs.CreerAdmin(admin);
                hvm.Identifiant = identifiant;

                return View("Index", hvm);
            }
            return View();

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
            HttpContext.SignOutAsync();
            return View();
        }

    }
}
