using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet2.Models.Boutique;
using Projet2.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2.Controllers
{
    public class BoutiqueController : Controller
    {
        // GET: /<controller>/
        public IActionResult AjouterArticle()
        {
            return View();

        }

        [HttpPost]
        public IActionResult AjouterArticle(string nom, string description, int prix, int stock, int prixTTC,IFormFile FileToUpload)
        {
            // mettre le file dans le dossier
            ArticleRessources ctx = new ArticleRessources();
            ctx.CreerArticle(nom, description, prix, stock, prixTTC /*, FileToUpload.FileName*/ );
            return View();

        }

        [HttpGet]
        public IActionResult ModifierArticle(int id)
        {
            ArticleRessources ctx = new ArticleRessources();
            Article article = ctx.ObtientTousLesArticles().Where(a => a.Id == id).FirstOrDefault();

            return View(article);

        }

        [HttpPost]
        public IActionResult ModifierArticle(int id, string nom, string description, decimal prix, int stock, decimal prixTTC)
        {
            ArticleRessources ctx = new ArticleRessources();
            ctx.ModifierArticle(id, nom, description, prix, stock, prixTTC);
            return RedirectToAction("ModifierArticle", new { @Id = id });

        }

        public IActionResult AfficherBoutique()
        {
            ArticleRessources ctx = new ArticleRessources();
            List<Article> articles = ctx.ObtientTousLesArticles();

            HomeViewModel hvm = new HomeViewModel
            {

                Boutiques = new Boutiques() { Articles = articles, NombreArticle = articles.Count },

            };
            return View(hvm);

        }

        public IActionResult Article(int id)
        {
            ArticleRessources ctx = new ArticleRessources();
            Article article = ctx.ObtientTousLesArticles().Where(a => a.Id == id).FirstOrDefault();

            HomeViewModel hvm = new HomeViewModel
            {

                Article = article

            };
            return View(hvm);

        }
        /*[HttpPost]
        public IActionResult Article(int id, int Quantite )
        {
           PanierService ctx = new PanierService();

            HttpContext context = HttpContext.Current;
            if (HttpContext.Session == null)
            {
                PanierBoutique panier = ctx.CreerPanier();
                HttpContext.Session = panier.Id;
            }
            ArticleRessources ctxarticle = new ArticleRessources();
            Article article = ctxarticle.ObtientTousLesArticles().Where(a => a.Id == id).FirstOrDefault();

            LignePanierBoutique ligne = new LignePanierBoutique() { Article = article, Quantite = Quantite, SousTotal = article.PrixTTC * Quantite };

            PanierBoutique panier = ctx.ObtientTousLesPaniers().Where(/* session est celle en cours  *);
            
            if (panier == null) // SI le panier n'existe pas on le crée
            {
                HttpContext.Session
            }
            else // Sinon
            {
                LignePanierBoutique lignePanier = ctx.ObtientTousLesLignes().Where(b => b.ArticleId == article.Id , c => c.PanierId == panier.Id);
                if (lignePanier.count != 0) // Si cet article est deja dans le panier:
                {

                    ctx.ModifierLigne(lignePanier.Id, lignePanier.Quantite + Quantite, lignePanier.Article, lignePanier.SousTotal+ (article.PrixTTC*Quantite));

                    ctx.ModifierPanier(panier.Id,);
                }
                else
                {
                    ctx.CreerLigne(Quantite, article, article.PrixTTC*Quantite);
                    ctx.
                }

            }


            ctx.ModifierPanier(Quantite, line);
            LignePanierBoutique ligne = ctx

            HomeViewModel hvm = new HomeViewModel
            {

                Article = article

            };
            return View(hvm);

        }*/


        public IActionResult Panier(int id , int Quantite)
        {
            ArticleRessources ctxarticle = new ArticleRessources();
            Article article = ctxarticle.ObtientTousLesArticles().Where(a => a.Id == id).FirstOrDefault();

            LignePanierBoutique ligne = new LignePanierBoutique() { Article = article, Quantite = Quantite, SousTotal = article.PrixTTC * Quantite };

           
            HomeViewModel hvm = new HomeViewModel
            {

                Article = article

            };
            return View(hvm);

        }


    }
}
