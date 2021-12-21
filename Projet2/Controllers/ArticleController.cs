using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2.Controllers
{
    public class ArticleController : Controller
    {
        // GET: /<controller>/
        public IActionResult AjouterArticle()
        {
            return View();

        }

        [HttpPost]
        public IActionResult AjouterArticle(string nom, string description, int prix, int stock, int prixTTC)
        {
            ArticleRessources ctx = new ArticleRessources();
            //ctx.CreerArticle(nom, description, prix, stock, prixTTC, );
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
        public IActionResult ModifierArticle(int id, string nom, string description, decimal prix, int stock, decimal prixTTC, AdP adp)
        {
            ArticleRessources ctx = new ArticleRessources();
            ctx.ModifierArticle(id, nom, description, prix, stock, prixTTC, adp.Id);
            return RedirectToAction("ModifierArticle", new {@Id = id });

        }

        public IActionResult AfficheBoutique()
        {
            return View();

        }
    }
}
