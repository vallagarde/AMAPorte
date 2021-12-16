using System;
using System.Collections.Generic;
using System.Linq;


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

        public int CreerLigne(int quantite, Article article, decimal sousTotal)
        {
            LignePanierBoutique ligne = new LignePanierBoutique() { Article=article, Quantite=quantite, SousTotal=sousTotal };
            _bddContext.LignePanierBoutique.Add(ligne);
            _bddContext.SaveChanges();
            return ligne.Id;

        }
        public int CreerPanier( List<LignePanierBoutique> liste)
        {
            decimal Total = 0;

            foreach (LignePanierBoutique ligne in liste)
            {
                Total += ligne.SousTotal;
            }
            PanierBoutique panier = new PanierBoutique() { LignePanierBoutiques= liste, Total=Total };
            _bddContext.PanierBoutique.Add(panier);
            _bddContext.SaveChanges();
            return panier.Id;

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int ModifierLigne(int id, int quantite, Article article, decimal sousTotal)
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
    }
}
