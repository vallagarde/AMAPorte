﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using Projet2.ViewModels;
using System.Linq;

namespace Projet2.Controllers
{
    public class EspaceAdPController : Controller
    {
        //ajouter des articles à la boutique OK les modifier OK  les supprimer OK, les afficher dans son espace personnel OK
        //voir les KPI sur les ventes, voir les commandes (historique + en cours(a preparer, a livrer))
        //ajouter des paniers s., les modifier, les supprimer (qu'avant une date specifique prennant en compte les commandes en amont) les afficher dans son espace personnel,
        //voir les KPI sur les ventes, voir les commandes (historique + en cours(a preparer, a livrer))
        //ajouter des ateliers, les modifier, les annuler, les afficher dans son espace personnel
        //(avec informations sur les participants (nom, prenom, telephone, /nom entreprise et nombre participants pour CE))

        CompteServices cs = new CompteServices();
        ArticleRessources ar = new ArticleRessources();
        HomeViewModel hvm = new HomeViewModel();
        public IActionResult Index()
        {
            return View();
        }


        //LIAISONS BOUTIQUE
        public IActionResult GestionBoutique(AdP adp)
        {
            ar.ObtenirArticleParAdP(adp);
            hvm.AdP = adp;
            return View(hvm);
        }

        [HttpGet]
        public IActionResult ArticleModification(Article article)
        {
            hvm.Article = article;
            return View(hvm);
        }
        [HttpPost]
        public IActionResult ArticleModification(Article article, AdP adp)
        {
            ArticleRessources ctx = new ArticleRessources();
            ctx.ModifierArticle(article.Id, article.Nom, article.Description, article.Prix, article.Stock, article.PrixTTC, adp.Id);

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdP == true)
                {
                    HomeViewModel hvm = new HomeViewModel();
                    hvm.AdP = cs.ObtenirAdPParIdentifiant(viewModel.Identifiant.Id);
                    hvm.Article.Id = article.Id;
                    return RedirectToAction("GestionBoutique", "EspaceAdP", hvm.AdP);
                }
            }
            return RedirectToAction("Index", "Login");

        }


        public IActionResult SupprimerArticle(Article article)
        {
            ArticleRessources ctx = new ArticleRessources();

            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdP == true)
                {
                    AdP adp = cs.ObtenirAdPParIdentifiant(viewModel.Identifiant.Id);
                    ctx.SupprimerArticle(article.Id);
                    return RedirectToAction("GestionBoutique", "EspaceAdP", adp);
                }
                return RedirectToAction("EspacePersonnel", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }



        }


    }
}