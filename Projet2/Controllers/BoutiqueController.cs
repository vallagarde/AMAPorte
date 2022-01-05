using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult AjouterArticle()
        {
            HomeViewModel hvm = new HomeViewModel
            {
                PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId")

            };
            return View(hvm);

        }

        [Authorize]
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


        public IActionResult AfficherBoutique()
        {
            ArticleRessources ctx = new ArticleRessources();
            CompteServices csc = new CompteServices();
            List<Article> articles = ctx.ObtientTousLesArticles();

            HomeViewModel hvm = new HomeViewModel
            {
                ListeComptesAdP = csc.ObtenirTousLesAdPs(),
                Boutiques = new Boutiques() { Articles = articles, NombreArticle = articles.Count },
                PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId")

            };
            return View(hvm);

        }
        [HttpPost]
        public IActionResult AfficherBoutique(String recherche, String rechercheAdP)
        {
            ArticleRessources ctx = new ArticleRessources();
            CompteServices csc = new CompteServices();
            List<Article> articles = ctx.ObtientTousLesArticles();
            List<Article>  articles2 = articles;
            List<Article> articles3 = new List<Article>();
            List<AdP> listAdP = null;

            if (recherche==null && rechercheAdP == null)
            {
                return RedirectToAction("AfficherBoutique");
            }
            else
            {
                if (rechercheAdP != null)
                {

                    listAdP = csc.ObtenirAdPParNom(rechercheAdP);
                }

                if (recherche != null)
                {
                    articles2 = articles.FindAll(x => x.Nom.ToLower().Contains(recherche.ToLower()));

                    if (listAdP != null)
                    {
                        foreach (AdP adp in listAdP)
                        {
                            List<Article> listintermediaire = articles2.FindAll(x => x.AdPId == adp.Id);

                            foreach (Article articleintermediaire in listintermediaire)
                            {
                                articles3.Add(articleintermediaire);

                            }
                        }
                    }
                    else
                    {
                        articles3 = articles2;
                    }
                }
                else if (listAdP != null)
                {
                    foreach (AdP adp in listAdP)
                    {
                        List<Article> listintermediaire = articles2.FindAll(x => x.AdPId == adp.Id);

                        foreach (Article articleintermediaire in listintermediaire)
                        {
                            articles3.Add(articleintermediaire);

                        }
                    }
                }


                HomeViewModel hvm = new HomeViewModel
                {
                    ListeComptesAdP = csc.ObtenirTousLesAdPs(),
                    Boutiques = new Boutiques() { Articles = articles3, NombreArticle = articles3.Count },
                    PanierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId")

                };
                return View(hvm);
            }
            

        }

        public IActionResult Article(int id)
        {
            ArticleRessources ctx = new ArticleRessources();
            Article article = ctx.ObtientTousLesArticles().Where(a => a.Id == id).FirstOrDefault();
            article.Avis = ctx.AfficherAvisPourArticle(article);

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
        
        public IActionResult SupprimerLigne(int Id)
        {
            int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");
            PanierService ctx = new PanierService();
            LignePanierBoutique ligne = ctx.ObtientLigne(Id);


            ctx.SupprimerLignePanier(ligne);

            return RedirectToAction("Panier", new { @panierId = panierId });

        }

        [HttpGet]
        public IActionResult ArticleAvis(LignePanierBoutique lignePanierBoutique)
        {
            HomeViewModel model = new HomeViewModel();

            ArticleRessources ctxarticle = new ArticleRessources();
            model.Article = ctxarticle.ObtientTousLesArticles().Where(a => a.Id == lignePanierBoutique.ArticleId).FirstOrDefault();
            model.LignePanierBoutique = lignePanierBoutique;

            return View(model);

        }

        [HttpPost]
        public IActionResult ArticleAvis(LignePanierBoutique lignePanierBoutique, Article article)
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, "authentification") };
            if (viewModel.Authentifie)
            {
                CompteServices cs = new CompteServices();
                HomeViewModel hvm = new HomeViewModel();
                ArticleRessources ctx = new ArticleRessources();
                viewModel.Identifiant = cs.ObtenirIdentifiant(HttpContext.User.Identity.Name);
                if (viewModel.Identifiant.EstAdA == true)
                {
                    hvm.AdA = cs.ObtenirAdAParIdentifiant(viewModel.Identifiant.Id);
                    
                    ctx.AjouterAvisAdA(lignePanierBoutique, article, hvm.AdA.Id);

                    return RedirectToAction("Article", new { @Id = article.Id });
                }
                else if (viewModel.Identifiant.EstCE == true)
                {
                    hvm.ContactComiteEntreprise = cs.ObtenirCCEParIdentifiant(viewModel.Identifiant.Id);
                    
                    ctx.AjouterAvisCE(lignePanierBoutique, article, hvm.ContactComiteEntreprise.EntrepriseId);

                    return RedirectToAction("Article", "Boutique", hvm.ContactComiteEntreprise);
                }
            }
            return RedirectToAction("Index", "Login");
        }


        public int QuantitePanier()
        {
            PanierService ctx = new PanierService();
            int ProduitsNombres = 0;
            int panierId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "panierId");
            PanierBoutique panierBoutique = ctx.ObientPanier(panierId);

            foreach (LignePanierBoutique ligne in panierBoutique.LignePanierBoutiques)
            {
                ProduitsNombres += ligne.Quantite;
            }
            return ProduitsNombres;
        }


    }
}
