using Projet2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Projet2.Models.Compte;

namespace ChoixSejour.Controllers
{
    public class LoginController : Controller
    {
        private CompteRessources cptressource;
        public LoginController()
        {
            cptressource = new CompteRessources();
        }
        public IActionResult Index()
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            if (viewModel.Authentifie)
            {
                viewModel.Identifiant = cptressource.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                return View(viewModel);
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(UtilisateurViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Identifiant identifiant = cptressource.Authentifier(viewModel.Identifiant.UserName, viewModel.Identifiant.MotDePasse);
                if (identifiant != null)
                {
                    var userClaims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, identifiant.Id.ToString()),
                    };

                    var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

                    var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });
                    HttpContext.SignInAsync(userPrincipal);

                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return Redirect("/");
                }
                ModelState.AddModelError("Identifiant.UserName", "Prénom et/ou mot de passe incorrect(s)");
            }
            return View(viewModel);
        }

        public IActionResult CreationIdentifiantProvisoire()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreationIdentifiantProvisoire(Identifiant identifiant)
        {
            if (ModelState.IsValid)
            {
                int id = cptressource.AjouterIdentifiant(identifiant.UserName, identifiant.MotDePasse);

                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, id.ToString()),
                };

                var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });
                HttpContext.SignInAsync(userPrincipal);

                return Redirect("/");
            }
            return View(identifiant);
        }

        public ActionResult Deconnexion()
        {
            HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
