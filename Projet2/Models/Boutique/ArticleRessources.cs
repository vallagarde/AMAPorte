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

    }
}
