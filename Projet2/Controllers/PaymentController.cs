using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
using Projet2.Models.Boutique;
using Projet2.Models.Calendriers;
using Projet2.Models.Compte;
using Projet2.Models.Mails;
using Projet2.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2.Controllers
{
    public class PaymentController : Controller
    {
        // GET: /<controller>/

        public IActionResult Paiement(HomeViewModel hvm)
        {
            PanierService ctx = new PanierService();
            int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");
            PanierBoutique panier = ctx.ObientPanier(panierId);

            ctx.ChangerEtatCommande(panierId, "EstEnPreparation");
            hvm.PanierBoutique = panier;
            hvm.PanierId = panierId;

            

            return View(hvm);
        }

        public IActionResult Commande()
        {
            PanierService ctx = new PanierService();
            CalendrierService csc = new CalendrierService();
            HomeViewModel hvm = new HomeViewModel();
            Commande commande = new Commande();
            commande.PanierBoutique = hvm.PanierBoutique;
            commande.DateTime = DateTime.Now;

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdA == true)
                {
                    commande.AdAId = cs.ObtenirAdAParIdentifiant(viewModel.Identifiant.Id).Id;

                }
                else if (viewModel.Identifiant.EstCE == true)
                {
                    commande.EntrepriseId = cs.ObtenirCCEParIdentifiant(viewModel.Identifiant.Id).Entreprise.Id;
                }

            }
            else
            {
                commande.ClientId = SessionHelper.GetObjectFromJson<Client>(HttpContext.Session, "client").Id;
            }

            ctx.CreerCommande(commande);
            csc.AjouterLigneCalendrierCommande(commande);

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
                    return RedirectToAction("Paiement", hvm);
                }
                else if (viewModel.Identifiant.EstAdA == true)
                {
                    hvm.AdA = cs.ObtenirAdAParIdentifiant(viewModel.Identifiant.Id);
                    return RedirectToAction("Paiement", hvm);
                }
                else if (viewModel.Identifiant.EstCE == true)
                {
                    hvm.ContactComiteEntreprise = cs.ObtenirCCEParIdentifiant(viewModel.Identifiant.Id);
                    return RedirectToAction("Paiement", hvm);
                }

            }
            return RedirectToAction("FicheRenseignement");
        }

        public IActionResult FicheRenseignement()
        {

            return View();

        }

        [HttpPost]
        public IActionResult FicheRenseignement(Client client, Adresse adresse)
        {

            CompteServices csx = new CompteServices();
            csx.CreerClient(client, adresse);
            HomeViewModel hvm = new HomeViewModel
            {
                Client = client
            };
            SessionHelper.SetObjectAsJson(HttpContext.Session, "client", client);
            return RedirectToAction("Paiement", new { hvm });

        }
    }

 
}
