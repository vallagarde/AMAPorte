using Microsoft.AspNetCore.Mvc;
using Projet2.Models.PanierSaisonniers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Controllers
{
    public class PanierController : Controller
    {

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
        public IActionResult AjouterPanier(List<Produit> produitsProposes, string description, string nomProducteur, decimal prix)
        {
            PanierSaisonnierService ctx = new PanierSaisonnierService();
            ctx.CreerPanierSaisonnier(produitsProposes, description, nomProducteur, prix);
            return View();

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
        public IActionResult ModifierPanier(int id, List<Produit> produitsProposes, string description, string nomProducteur, decimal prix)
        {
            PanierSaisonnierService ctx = new PanierSaisonnierService();
            ctx.ModifierPanierSaisonnier(id, produitsProposes, description, nomProducteur, prix);
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
