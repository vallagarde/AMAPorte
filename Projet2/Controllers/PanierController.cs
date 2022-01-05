using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Projet2.Helpers;
using Projet2.Models.Calendriers;
using Projet2.Models.Compte;
using Projet2.Models.PanierSaisonniers;
using Projet2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Controllers
{
    public class PanierController : Controller
    {
        private HomeViewModel hvm = new HomeViewModel();
        private PanierSaisonnierService ctx = new PanierSaisonnierService();
        private LignePanierService lignePanierService = new LignePanierService();
        private readonly IWebHostEnvironment _webHostEnvironment;
        private CalendrierService csv = new CalendrierService();

        public PanierController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult AfficherPaniers()
        {           
            hvm.CataloguePanier.PanierSaisonniers = new PanierSaisonnierService().ObtientTousLesPaniers();
            return View(hvm);
        }


        public IActionResult CommanderPaniers(int id)
        {
            hvm.PanierSaisonnier = ctx.ObtientTousLesPaniers().Where(p => p.Id == id).FirstOrDefault();
            return View(hvm);
        }

        [HttpPost]
        public IActionResult CommanderPaniers( int id, int quantite, int semaine)
        {
            LignePanierService ctxligne = new LignePanierService();
            int AdAId = 0;
            int EntrepriseId = 0;

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdA == true)
                {
                    AdAId = cs.ObtenirAdAParIdentifiant(viewModel.Identifiant.Id).Id;

                }
                else if (viewModel.Identifiant.EstCE == true)
                {
                    EntrepriseId = cs.ObtenirCCEParIdentifiant(viewModel.Identifiant.Id).Entreprise.Id;
                }

            }

            PanierSaisonnier panier = ctx.ObtientTousLesPaniers().Where(p => p.Id == id).FirstOrDefault();
            LignePanierSaisonnier lignePanier = ctxligne.CreerLignePanier(quantite, id, quantite * panier.Prix, semaine, AdAId, EntrepriseId);
            hvm.LignePanierSaisonnier = lignePanier;
            //return View(hvm);
            RouteValueDictionary Dict = new RouteValueDictionary();
            Dict.Add("lignePanier", lignePanier);

            return RedirectToAction("CommanderLignePanier", new { @lignepanierId = lignePanier.Id });
        }


        public IActionResult Paiement(int panierId)
        {

            LignePanierSaisonnier lignePanier = lignePanierService.ObtientLignePanierParId(panierId);
            CommandePanier commande = lignePanierService.CreerCommande(lignePanier);
            csv.AjouterLigneCalendrierPanier(commande);
            
            return View();
        }
      
        public IActionResult CommanderPanier(int Id)
        {

            hvm.PanierSaisonnier = ctx.ObtientTousLesPaniers().Where(p => p.Id == Id).FirstOrDefault();
            return View(hvm);
        }

        [HttpPost]
        public IActionResult CommanderPanier(int id, int quantite, int semaine)
        {
            LignePanierService ctxligne = new LignePanierService();

            int AdAId = 0;
            int EntrepriseId = 0;

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdA == true)
                {
                    AdAId = cs.ObtenirAdAParIdentifiant(viewModel.Identifiant.Id).Id;

                }
                else if (viewModel.Identifiant.EstCE == true)
                {
                    EntrepriseId = cs.ObtenirCCEParIdentifiant(viewModel.Identifiant.Id).Entreprise.Id;
                }

            }

            PanierSaisonnier panier = ctx.ObtientTousLesPaniers().Where(p => p.Id == id).FirstOrDefault();
            LignePanierSaisonnier lignePanier = ctxligne.CreerLignePanier(quantite, id, quantite * panier.Prix, semaine, AdAId, EntrepriseId);
            hvm.LignePanierSaisonnier = lignePanier;
            //return View(hvm);
            RouteValueDictionary Dict = new RouteValueDictionary();
            Dict.Add("lignePanier", lignePanier);

            return RedirectToAction("CommanderLignePanier", new { @lignepanierId = lignePanier.Id });
        }

        public IActionResult CommanderLignePanier(int lignepanierId)

        {
            LignePanierSaisonnier LignePanier = lignePanierService.ObtientLignePanierParId(lignepanierId);
            hvm.PanierSaisonnier = lignePanierService.ObtientPanierParId(LignePanier.PanierSaisonnierId);
            hvm.LignePanierSaisonnier = LignePanier;
            return View(hvm);
        }

        [HttpPost]
        public IActionResult CommandeLignePanier(PanierSaisonnier panierSaisonnier, int quantite)
        {
            hvm.PanierSaisonnier = ctx.ObtientTousLesPaniers().Where(p => p.Id == panierSaisonnier.Id).FirstOrDefault();



            return View(hvm);
        }

        [HttpGet]
        public IActionResult AjouterPanier()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AjouterPanier(PanierSaisonnier panierSaisonnier, IFormFile fileToUpload)
        {

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdP == true)
                {
                    AdP adp = cs.ObtenirAdPParIdentifiant(viewModel.Identifiant.Id);
                    panierSaisonnier.AdPId = adp.Id;
                    panierSaisonnier.Image = fileToUpload.FileName;
                    ctx.CreerPanierSaisonnier(panierSaisonnier);

                    // mettre le file dans le dossier
                    //var FileDic1 = "Files";

                    string FilePath1 = Path.Combine(_webHostEnvironment.WebRootPath, "ImageArticle");

                    if (!Directory.Exists(FilePath1))

                        Directory.CreateDirectory(FilePath1);

                    var fileName1 = fileToUpload.FileName;

                    var filePath1 = Path.Combine(FilePath1, fileName1);

                    using (FileStream fs = System.IO.File.Create(filePath1))

                    {
                        fileToUpload.CopyTo(fs);
                    }
                    return RedirectToAction("GestionPanier", "EspaceAdP", adp);

                }
            } return RedirectToAction("/");
          
        }

        [HttpGet]
        public IActionResult ModifierPanier(int id)
        {
            hvm.PanierSaisonnier = ctx.ObtientTousLesPaniers().Where(p => p.Id == id).FirstOrDefault();
            return View(hvm);
        }

        [HttpPost]
        public IActionResult ModifierPanier(PanierSaisonnier panierSaisonnier)
        {
            ctx.ModifierPanierSaisonnier(panierSaisonnier);
            return RedirectToAction("AfficherPaniers", panierSaisonnier.Id);
        }

        public IActionResult SupprimerPanier(PanierSaisonnier panierSaisonnier)
        {
            PanierSaisonnierService ctx = new PanierSaisonnierService();
            ctx.SupprimerPanierSaisonnier(panierSaisonnier.Id);
            return RedirectToAction("AfficherPaniers");
        }

        public bool PanierIsEmpty()
        {
            try
            {
                int PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");

                return PanierId == 0;
            }
            catch (Exception)
            {
                return true;
            }
        }

        [HttpGet]
        public IActionResult PanierAvis(LignePanierSaisonnier lignePanierSaisonnier)
        {
            HomeViewModel model = new HomeViewModel();

            PanierSaisonnierService panierSaisonnierService = new PanierSaisonnierService();
            model.PanierSaisonnier = panierSaisonnierService.ObtientTousLesPaniers().Where(a => a.Id == lignePanierSaisonnier.PanierSaisonnierId).FirstOrDefault();
            model.LignePanierSaisonnier = lignePanierSaisonnier;

            return View(model);

        }

        [HttpPost]
        public IActionResult PanierAvis(LignePanierSaisonnier lignePanierSaisonnier, PanierSaisonnier panierSaisonnier)
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                HomeViewModel hvm = new HomeViewModel();
                PanierSaisonnierService ctx = new PanierSaisonnierService();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdA == true)
                {
                    hvm.AdA = cs.ObtenirAdAParIdentifiant(viewModel.Identifiant.Id);

                    ctx.AjouterAvisAdA(lignePanierSaisonnier, panierSaisonnier, hvm.AdA.Id);

                    return RedirectToAction("HistoriqueCommandes", "CompteAdA", hvm.AdA);
                }
                else if (viewModel.Identifiant.EstCE == true)
                {
                    hvm.ContactComiteEntreprise = cs.ObtenirCCEParIdentifiant(viewModel.Identifiant.Id);

                    ctx.AjouterAvisCE(lignePanierSaisonnier, panierSaisonnier, hvm.ContactComiteEntreprise.EntrepriseId);

                    return RedirectToAction("HistoriqueCommandes", "CompteCE", hvm.ContactComiteEntreprise);
                }
            }
            return RedirectToAction("Index", "Login");
        }

    }
}
