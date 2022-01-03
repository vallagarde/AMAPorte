using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
using Projet2.Models.Compte;
using Projet2.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

namespace Projet2.Controllers
{
    [Authorize]
    public class CompteAdPController : Controller
    {
        //ajouter quelques attributs (RIB), warning avant suppression compte
        //adresse avec base de données ? 
       
        CompteServices cs = new CompteServices();
        HomeViewModel hvm = new HomeViewModel();

        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompteAdPController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(AdP adp)
        {
            hvm.Personne = cs.ObtenirPersonne(adp.PersonneId);

            if (hvm.Personne == null)
            {
                return View("Error");
            }
            else
            {
                hvm.Adresse = cs.ObtenirAdresse(hvm.Personne.AdresseId);
                hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);
                hvm.AdP = adp;
                return View(hvm);
            }
        }

        public IActionResult AdPCompteInfo(AdP adp)
        {

            hvm.Personne = cs.ObtenirPersonne(adp.PersonneId);

            if (hvm.Personne == null)
            {
                return View("Error");
            }
            else
            {
                hvm.Adresse = cs.ObtenirAdresse(hvm.Personne.AdresseId);
                hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);
                hvm.AdP = adp;
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
            if (!cs.TrouverIdentifiant(identifiant))
            {
                if (personne != null && identifiant != null && adresse != null)
                {

                    int Age = personne.getAge();
                    if (Age > 18)
                    {
                        personne.EstMajeur = true;
                    }

                    if (personne.EstMajeur == true)
                    {
                        if (personne.EstEnAccord == true)
                        {
                            adp = new AdP() { EstAdP = true };
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

                            return View("Index", hvm);
                        }
                    }
                    hvm.Personne = personne;
                    return View(hvm);
                }
            }
            hvm.AdresseExistante = cs.TrouverIdentifiant(identifiant);
            return View(hvm);
        }

        [HttpGet]
        public IActionResult AjouterImage(AdP adp)
        {
            hvm.AdP = adp;
            return View(hvm);
        }

        [HttpPost]
        public IActionResult AjouterImage(AdP adp, IFormFile FileToUpload)
        {
            adp.Image = FileToUpload.FileName;
            hvm.AdP = cs.ObtenirAdPParId(adp.Id);
            hvm.Personne = cs.ObtenirPersonne(hvm.AdP.PersonneId);

            cs.AjouterPhoto(FileToUpload.FileName, hvm.Personne.IdentifiantId);

            var FileDic = "Files";

            string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "ImageProfils");

            if (!Directory.Exists(FilePath))

                Directory.CreateDirectory(FilePath);

            var fileName = FileToUpload.FileName;

            var filePath = Path.Combine(FilePath, fileName);

            using (FileStream fs = System.IO.File.Create(filePath))

            {
                FileToUpload.CopyTo(fs);
            }
            return RedirectToAction("Index", hvm.AdA);
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
        public IActionResult ModificationCompte(Personne personne, Adresse adresse, AdP adp)
        {

            hvm.Adresse = cs.ModifierAdresse(adresse);
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
                    return View("AdPCompteInfo", hvm);
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
            //warning avant suppression compte
            cs.SupprimerAdP(adp.Id);
            bool EstUtilisateur = false;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "authentification", EstUtilisateur);
            HttpContext.SignOutAsync();
            return Redirect("/");
        }



    }
}
