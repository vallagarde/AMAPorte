using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PanierController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult AfficherPaniers()
        {           
            hvm.CataloguePanier.PanierSaisonniers = new PanierSaisonnierService().ObtientTousLesPaniers();
            return View(hvm);
        }
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

                    var FileDic1 = "Files";

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
            }

                ctx.CreerPanierSaisonnier(panierSaisonnier.NomPanier, panierSaisonnier.ProduitsProposes, panierSaisonnier.Description, panierSaisonnier.Prix, fileToUpload.FileName);

                var FileDic = "Files";

                string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "ImagesPaniers");

                if (!Directory.Exists(FilePath))
                    Directory.CreateDirectory(FilePath);

                var fileName = fileToUpload.FileName;
                var filePath = Path.Combine(FilePath, fileName);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    fileToUpload.CopyTo(fs);
                    return RedirectToAction("AfficherPaniers");
                }
          
        }

        [HttpGet]
        public IActionResult ModifierPanier(int id)
        {
            //if (id != 0)
            //{
            //    using (PanierSaisonnierService ctx = new PanierSaisonnierService()))
            //    {
            //        PanierSaisonnier panierSaisonnier = ctx.ObtientTousLesPaniers().Where(p => p.Id == id).FirstOrDefault();
            //        if (panierSaisonnier == null)
            //        {
            //            return View("Error");
            //        }
            //        return View();
            //    }
            //}
            //return View("Error");
            hvm.PanierSaisonnier = ctx.ObtientTousLesPaniers().Where(p => p.Id == id).FirstOrDefault();
            return View(hvm);
        }

        [HttpPost]
        public IActionResult ModifierPanier(PanierSaisonnier panierSaisonnier)
        {
            
            ctx.ModifierPanierSaisonnier(panierSaisonnier);
            return RedirectToAction("AfficherPaniers", panierSaisonnier.Id);
        }

        public IActionResult SupprimerPanier(int id)
        {
            PanierSaisonnierService ctx = new PanierSaisonnierService();
            ctx.SupprimerPanierSaisonnier(id);
            return RedirectToAction("AfficherPaniers");
        }
    }
}
