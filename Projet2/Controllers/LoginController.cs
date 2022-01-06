using Projet2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Projet2.Models.Compte;
using Projet2.Helpers;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Projet2.Controllers
{
    public class LoginController : Controller
        //ajouter des authentifications sur des espaces member only 

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
                            if (hvm.Identifiant.EstAdA)
                            {
                                if (hvm.AdA.EstAboAnnuel)
                                {
                                    int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");

                                    if (panierId != 0)
                                    {
                                        return RedirectToAction("Panier", "Boutique", new { @panierId = panierId });
                                    }
                                    return RedirectToAction("Index", "CompteAdA", hvm.AdA);

                                }
                                else if (!hvm.AdA.EstAboAnnuel) 
                                {
                                    return RedirectToAction("CreationCompte", "CompteAdA");
                                }
                                    
                            }
                        }
                        
                        if (hvm.AdP != null)
                        {
                            if (hvm.Identifiant.EstAdA)
                            {
                                if (hvm.AdP.EstAboAnnuel)
                                {
                                    int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");
                                    if (panierId != 0)
                                    {
                                        return RedirectToAction("Panier", "Boutique", new { @panierId = panierId });
                                    }
                                    return RedirectToAction("Index", "CompteAdP", hvm.AdP);
                                }
                                else if (!hvm.AdP.EstAboAnnuel)
                                {
                                    return RedirectToAction("CreationCompte", "CompteAdP");
                                }
                            }
                        }
                    }
                    
                    else if (hvm.ContactComiteEntreprise != null)
                    {
                        if (hvm.Identifiant.EstCE)
                        {

                            int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");
                            hvm.Entreprise = cptressource.ObtenirEntreprise(hvm.ContactComiteEntreprise.EntrepriseId);
                            if (hvm.Entreprise.EstAboAnnuel)
                            {
                                if (panierId != 0)
                                {
                                    return RedirectToAction("Panier", "Boutique", new { @panierId = panierId });
                                }

                                return RedirectToAction("Index", "CompteCE", hvm.ContactComiteEntreprise);
                            }
                            else if (!hvm.Entreprise.EstAboAnnuel)
                            {
                                return RedirectToAction("CreationCompte", "CompteCE");
                            }
                        }
                    }
                    
                    else if(hvm.Admin.Id != 0) 
                    {
                        if (hvm.Identifiant.EstGCRA || hvm.Identifiant.EstGCCQ || hvm.Identifiant.EstDSI)
                        {
                            return RedirectToAction("Index", "Admin", hvm.Admin);
                        }
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

        [Authorize]
        public ActionResult Deconnexion()
        {
            bool EstUtilisateur = false;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "authentification", EstUtilisateur);
            HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult ModificationIdentifiant(Identifiant Identifiant, int Id)
        {
            hvm.Identifiant = Identifiant;

            return View(hvm);
        }


        [HttpPost]
        public ActionResult ModificationIdentifiant(Identifiant identifiant)
        {
            CompteServices cs = new CompteServices();
            hvm.Identifiant = cs.ModifierIdentifiant(identifiant);
          
                HttpContext.SignOutAsync();
                 var userClaims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, hvm.Identifiant.Id.ToString()),
                    };
                var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });
                HttpContext.SignInAsync(userPrincipal);
               if(identifiant.Id != 0)
                {
                    if (hvm.Identifiant.EstAdP == true)
                    {
                        hvm.AdP = cs.ObtenirAdPParIdentifiant(hvm.Identifiant.Id);
                        return RedirectToAction("Index", "CompteAdP", hvm.AdP);
                    }
                    else if (hvm.Identifiant.EstAdA == true)
                    {
                        hvm.AdA = cs.ObtenirAdAParIdentifiant(hvm.Identifiant.Id);
                        return RedirectToAction("Index", "CompteAdA", hvm.AdA);
                    }
                    else if (hvm.Identifiant.EstCE == true)
                    {
                        hvm.ContactComiteEntreprise = cs.ObtenirCCEParIdentifiant(hvm.Identifiant.Id);
                        return RedirectToAction("Index", "CompteCE", hvm.ContactComiteEntreprise);
                    }
                    else if ((hvm.Identifiant.EstGCCQ == true) || (hvm.Identifiant.EstGCRA == true) || (hvm.Identifiant.EstDSI == true))
                    {
                        hvm.Admin = cs.ObtenirAdminParIdentifiant(hvm.Identifiant.Id);
                        return RedirectToAction("Index", "Admin", hvm.Admin);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                } else
                {
                //Envoie mail à l'adresse mail recu avec un lien vers la modification inclus identifiant Id
                return RedirectToAction("Index", "Login");
                }           
        }

        public bool UtilisateurEstConnecte()
        {

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                return true;
            }
            else return false;
        }

        public bool UtilisateurEstAdmin()
        {

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                viewModel.Identifiant = cptressource.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstGCRA || viewModel.Identifiant.EstGCCQ || viewModel.Identifiant.EstDSI)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
