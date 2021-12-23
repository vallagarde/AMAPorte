using Projet2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Projet2.Models.Compte;
using Projet2.Helpers;

namespace Projet2.Controllers
{
    public class LoginController : Controller
        //ajouter une option mot de passe oublié
        //ajouter des authentifications sur des espaces member only 
        //ajouter un identifiant avec un adresse mail (verifier si existe pas encore)

    {
        private CompteServices cptressource;
        private HomeViewModel hvm = new HomeViewModel();
        public LoginController()
        {
            cptressource = new CompteServices();
        }
        public IActionResult Index()
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            //UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
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
                Identifiant identifiant = cptressource.Authentifier(viewModel.Identifiant.AdresseMail, viewModel.Identifiant.MotDePasse);
                if (identifiant != null)
                {
                    var userClaims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, identifiant.Id.ToString()),
                    };

                    var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

                    var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });
                    HttpContext.SignInAsync(userPrincipal);

                    bool EstUtilisateur = true; 
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "authentification", EstUtilisateur);

                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    hvm.Identifiant = identifiant;
                    hvm.Personne = cptressource.ObtenirPersonneParIdentifiant(hvm.Identifiant.Id);           
                    hvm.ContactComiteEntreprise = cptressource.ObtenirCCEParIdentifiant(hvm.Identifiant.Id);
                    hvm.Admin = cptressource.ObtenirAdminParIdentifiant(hvm.Identifiant.Id);

                    if (hvm.Personne != null)
                    {
                        hvm.AdA = cptressource.ObtenirAdAParPersonne(hvm.Personne.Id);
                        hvm.AdP = cptressource.ObtenirAdPParPersonne(hvm.Personne.Id);
                        if (hvm.AdA != null)
                        {
                            if (hvm.AdA.Id != 0)
                            {
                                int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");

                                if(panierId != 0)
                                {
                                return RedirectToAction("Panier", "Boutique", new { @panierId = panierId });
                                }
                                return RedirectToAction("Index", "CompteAdA", hvm.AdA);
                            }
                        }
                        if (hvm.AdP != null)
                        {
                            if (hvm.AdP.Id != 0)
                            {
                                int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");
                                if (panierId != 0)
                                {
                                    return RedirectToAction("Panier", "Boutique", new { @panierId = panierId });
                                }
                                return RedirectToAction("Index", "CompteAdP", hvm.AdP);
                            }
                        }
                    }
                    else if (hvm.ContactComiteEntreprise != null)
                    {
                        int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");

                        if (panierId != 0)
                        {
                            return RedirectToAction("Panier", "Boutique", new { @panierId = panierId });
                        }

                        hvm.Entreprise = cptressource.ObtenirEntreprise(hvm.ContactComiteEntreprise.EntrepriseId);
                        return RedirectToAction("Index", "CompteCE", hvm.ContactComiteEntreprise);
                    }
                    else if(hvm.Admin.Id != 0) 
                    {
                        return RedirectToAction("Index", "Admin", hvm.Admin);
                    }
                    else
                    {
                        return Redirect("/");
                    }

                }
                ModelState.AddModelError("Identifiant.AdresseMail", "Mail et/ou mot de passe incorrect(s)");
            }
            return View(viewModel);
        }

        public ActionResult Deconnexion()
        {
            bool EstUtilisateur = false;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "authentification", EstUtilisateur);
            HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
