using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using Projet2.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

namespace Projet2.Controllers
{
    [Authorize]
    public class CompteAdAController : Controller
    {        
        CompteServices cs = new CompteServices();
        PanierService panierService = new PanierService();
        HomeViewModel hvm = new HomeViewModel();

        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompteAdAController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(AdA ada)
        {
            //voir les ateliers a venir sur sa page d'accueil 
            hvm.Personne = cs.ObtenirPersonne(ada.PersonneId);

            if (hvm.Personne == null)
            {
                UtilisateurViewModel viewModel = new UtilisateurViewModel() { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                hvm.AdA = cs.ObtenirAdAParIdentifiant(viewModel.Identifiant.Id);
                if(hvm.AdA == null) 
                {
                    return View("Error");
                } else
                {
                    hvm.Personne = cs.ObtenirPersonne(hvm.AdA.PersonneId);
                    hvm.Adresse = cs.ObtenirAdresse(hvm.Personne.AdresseId);
                    hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);
                    return View(hvm);
                }
            }
            else
            {
                hvm.AdA = cs.ObtenirAdAParId(ada.Id);
                hvm.Adresse = cs.ObtenirAdresse(hvm.Personne.AdresseId);
                hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);
                return View(hvm);
            }
        }

        public IActionResult AdACompteInfo(AdA ada)
        {

            hvm.Personne = cs.ObtenirPersonne(ada.PersonneId);

            if (hvm.Personne == null)
            {
                return View("Error");
            }
            else
            {
                hvm.Adresse = cs.ObtenirAdresse(hvm.Personne.AdresseId);
                hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);
                hvm.AdA = ada;
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
        public IActionResult CreationCompte(Personne personne, Identifiant identifiant, Adresse adresse)
        {
            if (!cs.TrouverIdentifiant(identifiant))
            {
                //adresse avec base de données ? 
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
                            AdA ada = new AdA() { EstAdA = true };
                            identifiant.EstAdA = ada.EstAdA;

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

                            hvm.AdA = cs.CreerAdA(personne, adresse, ada);
                            hvm.Personne = personne;
                            hvm.Adresse = adresse;
                            hvm.Identifiant = identifiant;

                            return RedirectToAction("Index", hvm);
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
        public IActionResult AjouterImage(AdA ada)
        {
            hvm.AdA = ada;
            return View(hvm);
        }

        [HttpPost]
        public IActionResult AjouterImage(AdA ada, IFormFile FileToUpload)
        {
            ada.Image = FileToUpload.FileName;
            hvm.AdA = cs.ObtenirAdAParId(ada.Id);
            hvm.Personne = cs.ObtenirPersonne(hvm.AdA.PersonneId);
            
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

        public IActionResult Commandes(AdA ada)
        {
            ada.CommandesBoutiqueEffectues = panierService.ObtenirCommandesParAdA(ada);
            hvm.AdA = ada;
            return View(hvm);
        }

        public IActionResult Commande_infos(int CommandeId)
        {
            Commande commande = panierService.ObtientCommandeParId(CommandeId);
            int panierId = commande.PanierBoutiqueId;
            PanierBoutique panierBoutique = panierService.ObientPanier(panierId);
            commande.PanierBoutique = panierBoutique;
            hvm.PanierBoutique = panierBoutique;
            hvm.Commande= commande;
            return View(hvm);
        }



        public IActionResult HistoriqueCommandes(AdA ada)
        {
            ada.CommandesBoutiqueEffectues = panierService.ObtenirCommandesParAdA(ada);
            hvm.AdA = ada;
            return View(hvm);
        }

        public IActionResult HistoriqueAteliers(AdA ada)
        {
            //voir l'historique des ateliers passés, ajouter un seul ! avis par atelier
            return View(hvm);
        }


        public IActionResult ArticlesFavoris(AdA ada)
        {
            //ajouter foncionnalité favoriser dans la boutique pour les utilisateurs connectés
            hvm.Personne = cs.ObtenirPersonne(ada.PersonneId);
            hvm.AdA = ada;
            return View(hvm);        
        }
        
              
        public IActionResult AteliersFavoris(AdA ada)
        {
            //avoir access a l'onglet ateliers pour en reserver/favoriser
            hvm.Personne = cs.ObtenirPersonne(ada.PersonneId);
            hvm.AdA = ada;
            return View(hvm);
        }

        public IActionResult ProducteursFavoris(AdA ada)
        {
            //ajouter foncionnalité favoriser dans les producteurs pour les utilisateurs connectés
            hvm.Personne = cs.ObtenirPersonne(ada.PersonneId);
            hvm.AdA = ada;
            return View(hvm);
        }

        [HttpGet]
        public IActionResult ModificationCompte(AdA ada)
        {
            hvm.AdA = cs.ObtenirAdAParId(ada.Id);
            hvm.Personne = cs.ObtenirPersonne(hvm.AdA.PersonneId);
            hvm.Adresse = cs.ObtenirAdresse(hvm.Personne.AdresseId);
            hvm.Identifiant = cs.ObtenirIdentifiant(hvm.Personne.IdentifiantId);

            return View(hvm);
        }

        [HttpPost]
        public IActionResult ModificationCompte(Personne personne, Adresse adresse, AdA ada)
        {
                
                hvm.Adresse = cs.ModifierAdresse(adresse);
                personne.AdresseId = hvm.Adresse.Id;
                personne.IdentifiantId = hvm.Identifiant.Id;
                hvm.Personne = cs.ModifierPersonne(personne);
                ada.PersonneId = hvm.Personne.Id;
                hvm.AdA = cs.ModifierAdA(ada);

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            if (viewModel.Authentifie)
            {
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdA == true)
                {
                    return View("AdACompteInfo", hvm);
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

        public IActionResult SuppressionCompte(AdA ada)
        {
            cs.SupprimerAdA(ada.Id);
            bool EstUtilisateur = false;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "authentification", EstUtilisateur);
            HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}
