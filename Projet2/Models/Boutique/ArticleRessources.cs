using Projet2.Models.Compte;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projet2.Models.Boutique
{
    public class ArticleRessources : IArticleRessources
    {
        private BddContext _bddContext;
        public ArticleRessources()
        {
            _bddContext = new BddContext();
        }

        public void DeleteCreateDatabase()
        {
            _bddContext.Database.EnsureDeleted();
            _bddContext.Database.EnsureCreated();
        }

        public List<Article> ObtientTousLesArticles()
        {
            return _bddContext.Article.ToList();
        }

        public void Dispose()
        {
            _bddContext.Dispose();
        }

        public List<Article> ObtenirArticleParAdP(AdP adp)
        {
            var queryArticle = from article in _bddContext.Article where article.AdPId == adp.Id select article;
            var articles = queryArticle.ToList();
                foreach (Article article in articles)
            {
                adp.Assortiment.Add(article);
            }
            return adp.Assortiment;
        }

        public int CreerArticle(string nom, string description, int prix, int stock, int prixTTC, String imageNom, int adpId)
        {
            Article article = new Article() { Nom = nom, Description=description, Prix=prix, PrixTTC=prixTTC, Stock=stock, Image= imageNom, AdPId = adpId };
            _bddContext.Article.Add(article);
            _bddContext.SaveChanges();
            return article.Id;
        }
        public int ModifierArticle(int Id ,string nom, string description, decimal prix, int stock, decimal prixTTC, int adpId)
        {
          
                Article article = _bddContext.Article.Find(Id);

                if (article != null)
                {                   
                    article.Id = Id;
                    article.Nom = nom;
                    article.Description = description;
                    article.Prix = prix;
                    article.PrixTTC = prixTTC;
                    article.Stock = stock;
                    article.AdPId = adpId;
                    article.EstValide = false;
                    article.EstEnAttente = false;
                _bddContext.SaveChanges();
                }          
                return article.Id;
        }

        public void SupprimerArticle(int Id)
        {
            Article articleASupprimer = _bddContext.Article.Find(Id);
            if (articleASupprimer != null)
            {
                _bddContext.Article.Remove(articleASupprimer);
                _bddContext.SaveChanges();
            }
        }

        public void ValidationArticle(Article article)
        {
            Article articleAValider = (from a in _bddContext.Article where a.Id == article.Id select a).FirstOrDefault();

            if (article.EstValide)
            {               
                articleAValider.EstValide = true;
                _bddContext.Update(articleAValider);
                _bddContext.SaveChanges();
            }
            else
            {
                articleAValider.AdminCommentaire = article.AdminCommentaire;
                articleAValider.EstEnAttente = true;
                _bddContext.Update(articleAValider);
                _bddContext.SaveChanges();
            }
        }

        public int CreerAvis(Avis avis)
        {
            _bddContext.Add(avis);
            _bddContext.SaveChanges();
            return avis.Id;
        }

        public void AjouterAvisAdA(LignePanierBoutique lignePanierBoutique, Article article, int Id)
        {
            Avis nouveauAvis = new Avis()
            {
                ArticleId = article.Id,
                Text = lignePanierBoutique.Avis.Text,
                Note = lignePanierBoutique.Avis.Note,
                AdAId = Id,
            };
            CreerAvis(nouveauAvis);
            lignePanierBoutique = _bddContext.LignePanierBoutique.Find(lignePanierBoutique.Id);
            lignePanierBoutique.AvisId = nouveauAvis.Id;
            _bddContext.LignePanierBoutique.Update(lignePanierBoutique);
            _bddContext.SaveChanges();
        }

        public void AjouterAvisCE(LignePanierBoutique lignePanierBoutique, Article article, int Id)
        {
            Avis nouveauAvis = new Avis()
            {
                ArticleId = article.Id,
                Text = lignePanierBoutique.Avis.Text,
                Note = lignePanierBoutique.Avis.Note,
                EntrepriseId = Id
            };
            CreerAvis(nouveauAvis);
            lignePanierBoutique = _bddContext.LignePanierBoutique.Find(lignePanierBoutique.Id);
            lignePanierBoutique.AvisId = nouveauAvis.Id;
            _bddContext.LignePanierBoutique.Update(lignePanierBoutique);
            _bddContext.SaveChanges();

        }

        public List<Avis> AfficherAvisPourArticle(Article article)
        {
            var queryAvis = from avis in _bddContext.Avis where avis.ArticleId == article.Id select avis;
            var avisArticle = queryAvis.ToList();
            foreach (Avis avis in avisArticle)
            {
                article.Avis.Add(avis);
            }
            return article.Avis;
        }

    }
}
