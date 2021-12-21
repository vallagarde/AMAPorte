using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet2.Models.PanierSaisonniers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Controllers
{
    public class PanierController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PanierController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult AfficherPaniers()
        {
            List<PanierSaisonnier> panierSaisonniers = new PanierSaisonnierService().ObtientTousLesPaniers();
            return View(panierSaisonniers);
        }
        public IActionResult AjouterPanier()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AjouterPanier(PanierSaisonnier panierSaisonnier, IFormFile fileToUpload)
        {
            PanierSaisonnierService ctx = new PanierSaisonnierService();
            ctx.CreerPanierSaisonnier(panierSaisonnier.NomPanier, panierSaisonnier.NomProducteur, panierSaisonnier.ProduitsProposes, panierSaisonnier.Description, panierSaisonnier.Prix, fileToUpload.FileName);

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
            PanierSaisonnierService ctx = new PanierSaisonnierService();
            PanierSaisonnier panierSaisonnier = ctx.ObtientTousLesPaniers().Where(p => p.Id == id).FirstOrDefault();
            return View(panierSaisonnier);
        }

        [HttpPost]
        public IActionResult ModifierPanier(int id, string nomPanier, string nomProducteur, string produitsProposes, string description, decimal prix)
        {
            PanierSaisonnierService ctx = new PanierSaisonnierService();
            ctx.ModifierPanierSaisonnier(id, nomPanier, nomProducteur, produitsProposes, description, prix);
            return RedirectToAction("AfficherPaniers", new { @Id = id });
        }

        public IActionResult SupprimerPanier(int id)
        {
            PanierSaisonnierService ctx = new PanierSaisonnierService();
            ctx.SupprimerPanierSaisonnier(id);
            return RedirectToAction("AfficherPaniers");
        }
    }
}
