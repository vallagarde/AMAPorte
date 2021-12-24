using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Projet2.Helpers;
using Projet2.Models.Compte;

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
            _bddContext.Dispose();
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
        public void CreerCommande(Commande commande)
        {
            _bddContext.Commande.Add(commande);
            _bddContext.SaveChanges();

        }

        public List<Commande> ObtenirCommandesParAdA(AdA ada)
        {
            var queryCommande = from commande in _bddContext.Commande where commande.AdAId == ada.Id select commande;
            var commandesAdA = queryCommande.ToList();
            foreach (Commande commande in commandesAdA)
            {
                var queryPanierBoutique = from panierBoutique in _bddContext.PanierBoutique where panierBoutique.Id == commande.PanierBoutiqueId select panierBoutique;
                commande.PanierBoutique = queryPanierBoutique.FirstOrDefault();
                var queryLignePanierBoutique = from lignePanierBoutique in _bddContext.LignePanierBoutique where lignePanierBoutique.PanierBoutiqueId == commande.PanierBoutique.Id select lignePanierBoutique;
                var lignes = queryLignePanierBoutique.ToList();
                foreach (LignePanierBoutique lignePanierBoutique in lignes)
                {
                    var queryArticle = from article in _bddContext.Article where article.Id == lignePanierBoutique.ArticleId select article;
                    lignePanierBoutique.Article = queryArticle.FirstOrDefault();
                    var queryAvis = from avis in _bddContext.Avis where avis.Id == lignePanierBoutique.AvisId select avis;
                    lignePanierBoutique.Avis = queryAvis.FirstOrDefault();
                }
                ada.CommandesBoutiqueEffectues.Add(commande);
            }
            return ada.CommandesBoutiqueEffectues;
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

        public int CreerAvis(Avis avis)
        {
            _bddContext.Add(avis);
            _bddContext.SaveChanges();
            return avis.Id;
        }

        

        public void AjouterAvisCE(LignePanierBoutique lignePanierBoutique, Article article, int Id)
        {
            Avis nouveauAvis = new Avis()
            {
                Article = article,
                Text = lignePanierBoutique.Avis.Text,
                Note = lignePanierBoutique.Avis.Note,
                EntrepriseId = Id
            };
            _bddContext.Add(nouveauAvis);
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

        public void ChangerEtatCommande(int panierId, string etat)
        {
            var queryCommande = from c in _bddContext.Commande where c.PanierBoutiqueId == panierId select c;
            Commande commande = queryCommande.FirstOrDefault();
            commande.EtatCommande = "";
            commande.EstEnPreparation = false;
            commande.EstARecuperer = false;
            commande.EstLivre = false;
            commande.EtatCommande = etat;
            _bddContext.Update(commande);
            _bddContext.SaveChanges();
        }

        public List<Commande> ObtenirCommandes()
        {
            List<Commande> commandes = _bddContext.Commande.ToList();
            foreach (Commande commande in commandes)
            {
                var queryAdA = from ada in _bddContext.AdAs where ada.Id == commande.AdAId select ada;
                commande.AdA = queryAdA.FirstOrDefault();
                var queryPersonne = from personne in _bddContext.Personnes where personne.Id == commande.AdA.PersonneId select personne;
                commande.AdA.Personne = queryPersonne.FirstOrDefault();
                var queryPanierBoutique = from panierBoutique in _bddContext.PanierBoutique where panierBoutique.Id == commande.PanierBoutiqueId select panierBoutique;
                commande.PanierBoutique = queryPanierBoutique.FirstOrDefault();
                var queryLignePanierBoutique = from lignePanierBoutique in _bddContext.LignePanierBoutique where lignePanierBoutique.PanierBoutiqueId == commande.PanierBoutique.Id select lignePanierBoutique;
                var lignes = queryLignePanierBoutique.ToList();
                foreach (LignePanierBoutique lignePanierBoutique in lignes)
                {
                    var queryArticle = from article in _bddContext.Article where article.Id == lignePanierBoutique.ArticleId select article;
                    lignePanierBoutique.Article = queryArticle.FirstOrDefault();
                    var queryAdP = from adp in _bddContext.AdPs where adp.Id == lignePanierBoutique.Article.AdPId select adp;
                    lignePanierBoutique.Article.AdP = queryAdP.FirstOrDefault();
                    var queryAvis = from avis in _bddContext.Avis where avis.Id == lignePanierBoutique.AvisId select avis;
                    lignePanierBoutique.Avis = queryAvis.FirstOrDefault();
                }               
            }
            return commandes;
        }

    }
}
