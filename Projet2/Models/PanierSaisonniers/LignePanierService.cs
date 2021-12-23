using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Models.PanierSaisonniers
{
    public class LignePanierService : ILignePanierSercice
    {
        private BddContext _bddContext;

        public int CreerLignePanier(int quantite, int panierSaisonnierId, decimal sousTotal)
        {
            LignePanierSaisonnier lignePanier = new LignePanierSaisonnier() { PanierSaisonnierId = panierSaisonnierId, Quantite = quantite, SousTotal = sousTotal };
            _bddContext.LignePanierSaisonniers.Add(lignePanier);
            _bddContext.SaveChanges();
            return lignePanier.Id;
        }
        
            public void Dispose()
        {
            _bddContext.Dispose();
        }
    }
}
