using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Projet2.Helpers;

namespace Projet2.Models.Boutique
{
    public class PanierService: IPanierService
    {
        private BddContext _bddContext;
        public PanierService()
        {
            _bddContext = new BddContext();
        }
        
        public List<PanierBoutique> ObtientTousLesPaniers()
        {
            return _bddContext.PanierBoutique.ToList();
        }

        public List<LignePanierBoutique> ObtientTousLesLignes()
        {
            return _bddContext.LignePanierBoutique.ToList();
        }
        public LignePanierBoutique ObtientLigne(int Id)
        {
            return _bddContext.LignePanierBoutique.Find(Id);
        }

        public Article ObtientArticle(int Id)
        {
            return _bddContext.Article.Find(Id);
        }


        public int CreerLigne(int quantite, int ArticleId, decimal sousTotal)
        {
            
            LignePanierBoutique ligne = new LignePanierBoutique() { ArticleId=ArticleId, Quantite=quantite, SousTotal=sousTotal };
            _bddContext.LignePanierBoutique.Add(ligne);
            _bddContext.SaveChanges();
            return ligne.Id;

        }
        public int CreerPanier()
        {
            PanierBoutique panier = new PanierBoutique() { LignePanierBoutiques = new List<LignePanierBoutique>() };
            _bddContext.PanierBoutique.Add(panier);
            _bddContext.SaveChanges();
            return panier.Id;

        }
        public PanierBoutique ObientPanier(int panierId)
        {

            return _bddContext.PanierBoutique.Include(c => c.LignePanierBoutiques).ThenInclude(it => it.Article).Where(c => c.Id == panierId).FirstOrDefault();
        }

        public void AjouterArticle(int PanierId, int ArticleId, int quantite)
        {
            ArticleRessources ctxarticle = new ArticleRessources();
            Article article = ctxarticle.ObtientTousLesArticles().Where(a => a.Id == ArticleId).FirstOrDefault();
            List<LignePanierBoutique> list = _bddContext.LignePanierBoutique.Where(a => a.ArticleId == ArticleId && a.PanierBoutiqueId == PanierId).ToList();
            int ligneId = 0;

            if (list.Count == 0)
            {
                ligneId = CreerLigne(quantite, ArticleId, quantite * article.PrixTTC);

            }
            else
            {
                LignePanierBoutique ligne = _bddContext.LignePanierBoutique.Where(a => a.ArticleId == ArticleId).Where(c => c.PanierBoutiqueId == PanierId).FirstOrDefault();
                ligneId = ModifierLigneRelatif(ligne.Id ,quantite,article, quantite * article.PrixTTC);
            }
            //LignePanierBoutique ligne = _bddContext.LignePanierBoutique.
            LignePanierBoutique ligneFinal = _bddContext.LignePanierBoutique.Find(ligneId);
            PanierBoutique panier = _bddContext.PanierBoutique.Find(PanierId);
            ligneFinal.PanierBoutiqueId = panier.Id;
                
             

            _bddContext.SaveChanges();
        }
        public void AjouterArticleAbsolue(int PanierId, int ArticleId, int quantite)
        {
            ArticleRessources ctxarticle = new ArticleRessources();
            Article article = ctxarticle.ObtientTousLesArticles().Where(a => a.Id == ArticleId).FirstOrDefault();
            List<LignePanierBoutique> list = _bddContext.LignePanierBoutique.Where(a => a.ArticleId == ArticleId && a.PanierBoutiqueId == PanierId).ToList();
            int ligneId = 0;

            if (list.Count == 0)
            {
                ligneId = CreerLigne(quantite, ArticleId, quantite * article.PrixTTC);

            }
            else
            {
                LignePanierBoutique ligne = _bddContext.LignePanierBoutique.Where(a => a.ArticleId == ArticleId).Where(c => c.PanierBoutiqueId == PanierId).FirstOrDefault();
                ligneId = ModifierLigneAbsolue(ligne.Id, quantite, article, quantite * article.PrixTTC);
            }
            //LignePanierBoutique ligne = _bddContext.LignePanierBoutique.
            LignePanierBoutique ligneFinal = _bddContext.LignePanierBoutique.Find(ligneId);
            PanierBoutique panier = _bddContext.PanierBoutique.Find(PanierId);
            ligneFinal.PanierBoutiqueId = panier.Id;



            _bddContext.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int ModifierLigneRelatif(int id, int quantite, Article article, decimal sousTotal)
        {
            LignePanierBoutique ligne = _bddContext.LignePanierBoutique.Find(id);

            if (ligne != null)
            {

                ligne.Id = id;
                ligne.Quantite += quantite ;
                ligne.SousTotal += sousTotal;

                _bddContext.SaveChanges();
            }


            return ligne.Id;
        }
        public int ModifierLigneAbsolue(int id, int quantite, Article article, decimal sousTotal)
        {
            LignePanierBoutique ligne = _bddContext.LignePanierBoutique.Find(id);

            if (ligne != null)
            {

                ligne.Id = id;
                ligne.Quantite = quantite;
                ligne.SousTotal = sousTotal;

                _bddContext.SaveChanges();
            }


            return ligne.Id;
        }

        public int ModifierPanier(int id, List<LignePanierBoutique> liste)
        {
            PanierBoutique panier = _bddContext.PanierBoutique.Find(id);

            decimal Total = 0;

            foreach (LignePanierBoutique ligne in liste)
            {
                Total += ligne.SousTotal;
            }

            if (panier != null)
            {

                panier.Id = id;
                panier.LignePanierBoutiques = liste;
                panier.Total = Total;
                

                _bddContext.SaveChanges();
            }


            return panier.Id;

        }
        
        public void SupprimerLignePanier(LignePanierBoutique ligne)
        {

           
            if (ligne != null)
            {
                _bddContext.LignePanierBoutique.Remove(ligne);

                _bddContext.SaveChanges();
            }
        }

        public void ViderPanier(PanierBoutique panier)
        {
            int panierId = panier.Id;
            List<LignePanierBoutique> list = _bddContext.LignePanierBoutique.Where(a => a.PanierBoutiqueId == panierId ).ToList();
            foreach (LignePanierBoutique ligne in list)
            {
                SupprimerLignePanier(ligne);
                
            }
            _bddContext.SaveChanges();
        }
        public void CalculerTotalPanier(int id)
        {
            PanierBoutique panier = _bddContext.PanierBoutique.Find(id);
            List<LignePanierBoutique> lignes = _bddContext.LignePanierBoutique.Where(a => a.PanierBoutiqueId == id).ToList();
            panier.Total = 0;
            foreach (LignePanierBoutique ligne in lignes)
            {
                panier.Total += ligne.SousTotal;
            }
            _bddContext.SaveChanges();

        }

    }
}
