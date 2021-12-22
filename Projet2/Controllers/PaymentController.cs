using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using Projet2.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2.Controllers
{
    public class PaymentController : Controller
    {
        // GET: /<controller>/

        public IActionResult Paiement(int Id)
        {
            PanierService ctx = new PanierService();
            int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");
            PanierBoutique panier = ctx.ObientPanier(panierId);
            HomeViewModel hvm = new HomeViewModel
            {
                PanierBoutique = panier,
                PanierId = panierId

            };
            return View(hvm);
        }

        [HttpPost]
        public IActionResult Paiement()
        {
            PanierService ctx = new PanierService();
            int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");
            PanierBoutique panier = ctx.ObientPanier(panierId);
            HomeViewModel hvm = new HomeViewModel
            {
                PanierBoutique = panier,
                PanierId = panierId

            };
            return View(hvm);
        }

        public IActionResult EtapePaiement(int Total)
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                HomeViewModel hvm = new HomeViewModel();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdP == true)
                {
                    hvm.AdP = cs.ObtenirAdPParIdentifiant(viewModel.Identifiant.Id);
                    return RedirectToAction("Paiement", hvm.AdP.Id);
                }
                else if (viewModel.Identifiant.EstAdA == true)
                {
                    hvm.AdA = cs.ObtenirAdAParIdentifiant(viewModel.Identifiant.Id);
                    return RedirectToAction("Paiement", hvm.AdA.Id);
                }
                else if (viewModel.Identifiant.EstCE == true)
                {
                    hvm.ContactComiteEntreprise = cs.ObtenirCCEParIdentifiant(viewModel.Identifiant.Id);
                    return RedirectToAction("Paiement", hvm.ContactComiteEntreprise.Id);
                }

            }
            return RedirectToAction("FicheRenseignement");
        }

        public IActionResult FicheRenseignement()
        {

            return View();

        }

        [HttpPost]
        public IActionResult FicheRenseignement(Personne personne, Adresse adresse)
        {

            CompteServices csx = new CompteServices();
            personne.Adresse = adresse;
            csx.CreerAdresse(adresse);
            csx.CreerPersonne(personne);
            return RedirectToAction("Paiement", "Personne", personne.Id);

        }
    }

 
}
