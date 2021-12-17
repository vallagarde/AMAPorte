using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
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

        [HttpPost]
        public IActionResult Article( int id, int Quantite)
        {
            int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");

            PanierService ctx = new PanierService();
            ArticleRessources ctxarticle = new ArticleRessources();

            Article article = ctxarticle.ObtientTousLesArticles().Where(a => a.Id == id).FirstOrDefault();

            //LignePanierBoutique ligne = new LignePanierBoutique() { Article = article, Quantite = Quantite, SousTotal = article.PrixTTC * Quantite };


            if (panierId == 0) // Le panier n'existe pas dans la session
            {
                panierId = ctx.CreerPanier();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "panierId", panierId);
                ctx.AjouterArticle(panierId, id, Quantite);

            }
            else // Sinon Le panier existe deja :
            {
                //PanierBoutique panier = ctx.ObientPanier(panierId);
                ctx.AjouterArticle(panierId, id, Quantite);
            }

            return RedirectToAction("Panier", new { @panierId = panierId });

        }


        public IActionResult Panier(int panierId)
        {
            PanierService ctx = new PanierService();
            PanierBoutique panier = ctx.ObientPanier(panierId);
            List < LignePanierBoutique > liste = panier.LignePanierBoutiques ;
            return View(panier);

        }

        public IActionResult ViderPanier()
        {
            int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");
            PanierService ctx = new PanierService();
            PanierBoutique panier = ctx.ObientPanier(panierId);
            ctx.ViderPanier(panier);

            return RedirectToAction("Panier", new { @panierId = panierId });

        }
    }
}
