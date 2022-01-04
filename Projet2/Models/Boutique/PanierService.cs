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


        public int CreerLigne(int quantite, int ArticleId, decimal sousTotal, int panierId)
        {
            
            LignePanierBoutique ligne = new LignePanierBoutique() { ArticleId=ArticleId, Quantite=quantite, SousTotal=sousTotal, PanierBoutiqueId = panierId };
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
                ligneId = CreerLigne(quantite, ArticleId, quantite * article.PrixTTC, PanierId);

            }
            else
            {
                LignePanierBoutique ligne = _bddContext.LignePanierBoutique.Where(a => a.ArticleId == ArticleId).Where(c => c.PanierBoutiqueId == PanierId).FirstOrDefault();
                ligneId = ModifierLigneRelatif(ligne.Id ,quantite,article, quantite * article.PrixTTC, PanierId);
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
                ligneId = CreerLigne(quantite, ArticleId, quantite * article.PrixTTC, PanierId);
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

        public int ModifierLigneRelatif(int id, int quantite, Article article, decimal sousTotal, int panierId)
        {
            LignePanierBoutique ligne = _bddContext.LignePanierBoutique.Find(id);

            if (ligne != null)
            {

                ligne.Id = id;
                ligne.Quantite += quantite ;
                ligne.SousTotal += sousTotal;
                ligne.PanierBoutiqueId = panierId;
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
                panier.Total += Decimal.Round(ligne.SousTotal, 2);
            }
            _bddContext.SaveChanges();

        }
        public void CreerCommande(Commande commande, int panierBoutiqueId)
        {

            commande.PanierBoutiqueId = panierBoutiqueId;
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

        public List<Commande> ObtenirCommandesParEntreprise(Entreprise entreprise)
        {
            var queryCommande = from commande in _bddContext.Commande where commande.EntrepriseId == entreprise.Id select commande;
            var commandesCE = queryCommande.ToList();
            foreach (Commande commande in commandesCE)
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
                entreprise.CommandesBoutiqueEffectues.Add(commande);
            }
            return entreprise.CommandesBoutiqueEffectues;
        }

        public void ChangerEtatCommande(int panierId, string etat)
        {
            var queryCommande = from c in _bddContext.Commande where c.PanierBoutiqueId == panierId select c;
            Commande commande = queryCommande.FirstOrDefault();
            commande.EtatCommande = "";
            commande.EstEnPreparation = false;
            commande.EstEnLivraison = false;
            commande.EstARecuperer = false;
            commande.EstLivre = false;
            commande.EtatCommande = etat;
            _bddContext.Update(commande);
            _bddContext.SaveChanges();
        }

        public List<Commande> ObtenirToutesCommandes()
        {
            List<Commande> commandes = _bddContext.Commande.ToList();
            foreach (Commande commande in commandes)
            {
                var queryAdA = from ada in _bddContext.AdAs where ada.Id == commande.AdAId select ada;
                commande.AdA = queryAdA.FirstOrDefault();
                if(commande.AdA != null)
                {
                    var queryPersonne = from personne in _bddContext.Personnes where personne.Id == commande.AdA.PersonneId select personne;
                    commande.AdA.Personne = queryPersonne.FirstOrDefault();
                    var queryAdresseP = from adresse in _bddContext.Adresses where adresse.Id == commande.AdA.Personne.AdresseId select adresse;
                    commande.AdA.Personne.Adresse = queryAdresseP.FirstOrDefault();
                }

                var queryCCE = from entreprise in _bddContext.Entreprises where entreprise.Id == commande.EntrepriseId select entreprise;
                commande.Entreprise = queryCCE.FirstOrDefault();
                if(commande.Entreprise != null)
                {
                    var queryAdresseE = from adresse in _bddContext.Adresses where adresse.Id == commande.Entreprise.AdresseId select adresse;
                    commande.Entreprise.Adresse = queryAdresseE.FirstOrDefault();
                }

                var queryClient = from client in _bddContext.Clients where client.Id == commande.ClientId select client;
                commande.Client = queryClient.FirstOrDefault();
                if (commande.Client != null)
                {
                    var queryAdresseC = from adresse in _bddContext.Adresses where adresse.Id == commande.Client.AdresseId select adresse;
                    commande.Client.Adresse = queryAdresseC.FirstOrDefault();
                }
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

        public List<Commande> ObtenirCommandesAdP(int adpId)
        {
            List<Commande> commandes = new List<Commande>();

            var queryLignes = from ligne in _bddContext.LignePanierBoutique where ligne.Article.AdPId == adpId select ligne;
            List<LignePanierBoutique> lignes = queryLignes.ToList();
            foreach (LignePanierBoutique lignePanierBoutique in lignes)
            {
                var queryArticle = from article in _bddContext.Article where article.Id == lignePanierBoutique.ArticleId select article;
                lignePanierBoutique.Article = queryArticle.FirstOrDefault();
                var queryAdP = from adp in _bddContext.AdPs where adp.Id == adpId select adp;
                lignePanierBoutique.Article.AdP = queryAdP.FirstOrDefault();
                var queryAvis = from avis in _bddContext.Avis where avis.Id == lignePanierBoutique.AvisId select avis;
                lignePanierBoutique.Avis = queryAvis.FirstOrDefault();

                var queryCommande = from c in _bddContext.Commande where c.PanierBoutiqueId == lignePanierBoutique.PanierBoutiqueId select c;
                List<Commande> ligneCommande = queryCommande.ToList();

                foreach (Commande commande in ligneCommande)
                {
                    var queryAdA = from ada in _bddContext.AdAs where ada.Id == commande.AdAId select ada;
                    commande.AdA = queryAdA.FirstOrDefault();
                    if (commande.AdA != null)
                    {
                        var queryPersonne = from personne in _bddContext.Personnes where personne.Id == commande.AdA.PersonneId select personne;
                        commande.AdA.Personne = queryPersonne.FirstOrDefault();
                        var queryAdresseP = from adresse in _bddContext.Adresses where adresse.Id == commande.AdA.Personne.AdresseId select adresse;
                        commande.AdA.Personne.Adresse = queryAdresseP.FirstOrDefault();
                    }

                    var queryCCE = from entreprise in _bddContext.Entreprises where entreprise.Id == commande.EntrepriseId select entreprise;
                    commande.Entreprise = queryCCE.FirstOrDefault();
                    if (commande.Entreprise != null)
                    {
                        var queryAdresseE = from adresse in _bddContext.Adresses where adresse.Id == commande.Entreprise.AdresseId select adresse;
                        commande.Entreprise.Adresse = queryAdresseE.FirstOrDefault();
                    }

                    var queryClient = from client in _bddContext.Clients where client.Id == commande.ClientId select client;
                    commande.Client = queryClient.FirstOrDefault();
                    if (commande.Client != null)
                    {
                        var queryAdresseC = from adresse in _bddContext.Adresses where adresse.Id == commande.Client.AdresseId select adresse;
                        commande.Client.Adresse = queryAdresseC.FirstOrDefault();
                    }

                    var queryPanierBoutique = from panierBoutique in _bddContext.PanierBoutique where panierBoutique.Id == commande.PanierBoutiqueId select panierBoutique;
                    commande.PanierBoutique = queryPanierBoutique.FirstOrDefault();

                    bool alreadyExist = commandes.Contains(commande);
                    if(!alreadyExist) commandes.Add(commande);
                }
            }
            return commandes;
        }

        public void DeduireDuStock(PanierBoutique panierBoutique)
        {
            
            foreach (LignePanierBoutique ligne in panierBoutique.LignePanierBoutiques)
            {
              
                int quantite = ligne.Quantite;
                int stock = ligne.Article.Stock;
                int articleId = ligne.ArticleId;
                Article article = ObtientArticle(articleId);

                int nouveauStock = stock - quantite;
                article.Stock = nouveauStock;
                _bddContext.SaveChanges();

            }
        }



        public int ArticlesPlusEnStock(PanierBoutique panier)
        {
            int articleid = 0;
            foreach(LignePanierBoutique ligne in panier.LignePanierBoutiques)
            {
                if(ligne.Article.Stock < ligne.Quantite)
                {
                    articleid = ligne.ArticleId;
                    break;
                }
            }

            return articleid;

        }
    }
}
