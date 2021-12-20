using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Projet2.Models.Compte;
using Projet2.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Projet2.Controllers
{
    public class AdminController : Controller
    {

        //TOUS LES VUES LIE A GESTION : GCRA; GCCQ; DSI
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

        [HttpGet]
        public IActionResult CreationCompte()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreationCompte(Admin admin, Identifiant identifiant)
        {

            if (admin != null && identifiant != null)
            {
                int id = cs.AjouterIdentifiant(identifiant.AdresseMail, identifiant.MotDePasse);

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
