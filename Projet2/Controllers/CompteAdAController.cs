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

        public IActionResult Index()
        {
            return View();
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

                cs.CreerAdA(personne, identifiant, adresse);
                //normally redirected to an overview but not the modify function
                return RedirectToAction("Index", new { @id = personne.Id });
            }
            return View();

        }

    }
}
