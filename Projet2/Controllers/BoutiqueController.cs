using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet2.Helpers;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using Projet2.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2.Controllers
{
    public class BoutiqueController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BoutiqueController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        // GET: /<controller>/
        public IActionResult AjouterArticle()
        {
            HomeViewModel hvm = new HomeViewModel
            {
                PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId")

            };
            return View(hvm);

        }


        [HttpPost]
        public IActionResult AjouterArticle(string nom, string description, int prix, int stock, int prixTTC, IFormFile FileToUpload)
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
                    ctx.CreerArticle(nom, description, prix, stock, prixTTC, FileToUpload.FileName, adp.Id);
                
            
            
                    // mettre le file dans le dossier

                    //var FileDic = "Files";

                    string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "ImageArticle");

                    if (!Directory.Exists(FilePath))

                        Directory.CreateDirectory(FilePath);

                    var fileName = FileToUpload.FileName;

                    var filePath = Path.Combine(FilePath, fileName);

                        using (FileStream fs = System.IO.File.Create(filePath))

                        {

                            FileToUpload.CopyTo(fs);
                        }
                        return RedirectToAction("GestionBoutique", "EspaceAdP", adp);


                        }
                } return RedirectToAction("Index", "Login");            

        }

        [HttpGet]
        public IActionResult ModifierArticle(int id)
        {
            ArticleRessources ctx = new ArticleRessources();
            Article article = ctx.ObtientTousLesArticles().Where(a => a.Id == id).FirstOrDefault();

            HomeViewModel hvm = new HomeViewModel
            {
                Article = article,
                PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId")

            };


            return View(hvm);

        }

        [HttpPost]
        public IActionResult ModifierArticle(int id, string nom, string description, decimal prix, int stock, decimal prixTTC, AdP adp)
        {
            ArticleRessources ctx = new ArticleRessources();
            ctx.ModifierArticle(id, nom, description, prix, stock, prixTTC, adp.Id);

            return RedirectToAction("ModifierArticle", new { @Id = id });

        }

        

        public IActionResult AfficherBoutique()
        {
            ArticleRessources ctx = new ArticleRessources();
            List<Article> articles = ctx.ObtientTousLesArticles();

            HomeViewModel hvm = new HomeViewModel
            {

                Boutiques = new Boutiques() { Articles = articles, NombreArticle = articles.Count },
                PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId")

            };
            return View(hvm);

        }
        [HttpPost]
        public IActionResult AfficherBoutique(String recherche)
        {
            ArticleRessources ctx = new ArticleRessources();
            List<Article> articles = ctx.ObtientTousLesArticles();

            List<Article> articles2 = articles.FindAll(x => x.Nom.Contains(recherche));

            HomeViewModel hvm = new HomeViewModel
            {

                Boutiques = new Boutiques() { Articles = articles2, NombreArticle = articles2.Count },
                PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId")

            };
            return View(hvm);

        }

        public IActionResult Article(int id)
        {
            ArticleRessources ctx = new ArticleRessources();
            Article article = ctx.ObtientTousLesArticles().Where(a => a.Id == id).FirstOrDefault();

            HomeViewModel hvm = new HomeViewModel
            {

                Article = article,
                PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId")

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
            ctx.CalculerTotalPanier(panierId);

            HomeViewModel hvm = new HomeViewModel
            {

                PanierBoutique = panier,
                PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId")

            };
            return View(hvm);

            

        }
        public IActionResult IPanier()
        {
            int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");

            return RedirectToAction("Panier", new { @panierId = panierId });

        }

        public IActionResult ViderPanier()
        {
            int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");
            PanierService ctx = new PanierService();
            PanierBoutique panier = ctx.ObientPanier(panierId);
            ctx.ViderPanier(panier);

            return RedirectToAction("Panier", new { @panierId = panierId });

        }

        [HttpPost]
        public IActionResult ModifierPanier(int quantite, int Id)
        {
            int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");
            PanierService ctx = new PanierService();
            LignePanierBoutique ligne = ctx.ObtientLigne(Id);
            Article article = ctx.ObtientArticle(ligne.ArticleId);


            ctx.ModifierLigneAbsolue(Id, quantite , article, article.PrixTTC*quantite );

            return RedirectToAction("Panier", new { @panierId = panierId });

        }
        [HttpPost]
        public IActionResult SupprimerLigne(int Id)
        {
            int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");
            PanierService ctx = new PanierService();
            LignePanierBoutique ligne = ctx.ObtientLigne(Id);


            ctx.SupprimerLignePanier(ligne);

            return RedirectToAction("Panier", new { @panierId = panierId });

        }
    }
}
