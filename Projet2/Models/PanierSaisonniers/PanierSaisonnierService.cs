using Microsoft.EntityFrameworkCore;
using Projet2.Models.Compte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Models.PanierSaisonniers
{
    public class PanierSaisonnierService : IPanierSaisonnierService
    {
        private BddContext _bddContext;
        public PanierSaisonnierService()
        {
            _bddContext = new BddContext();
        }

        public void DeleteCreateDatabase()
        {
            _bddContext.Database.EnsureDeleted();
            _bddContext.Database.EnsureCreated();
        }

        public List<PanierSaisonnier> ObtientTousLesPaniers()
        {
            return _bddContext.PanierSaisonniers.Include(p => p.AdP).ToList();
            //return _bddContext.PanierSaisonniers.ToList();
        }

        public void Dispose()
        {
            _bddContext.Dispose();
        }

        public List<PanierSaisonnier> ObtenirPanierParAdP(AdP adp)
        {
            var queryPanier = from panier in _bddContext.PanierSaisonniers where panier.AdPId == adp.Id select panier;
            var paniers = queryPanier.ToList();
            foreach (PanierSaisonnier panierSaisonnier in paniers)
            {
                adp.AssortimentPanier.Add(panierSaisonnier);
            }
            return adp.AssortimentPanier;
        }

        public int CreerPanierSaisonnier(string nomPanier, string produitsProposes, string description, decimal prix, string imagePanier, int adpId)
        {
            PanierSaisonnier panierSaisonnier = new PanierSaisonnier() { NomPanier = nomPanier,  ProduitsProposes = produitsProposes, Description = description, Prix = prix, Image = imagePanier, AdPId = adpId};
            _bddContext.PanierSaisonniers.Add(panierSaisonnier);
            _bddContext.SaveChanges();
            return panierSaisonnier.Id;
        }

        public int CreerPanierSaisonnier(PanierSaisonnier panierSaisonnier)
        {
            _bddContext.PanierSaisonniers.Add(panierSaisonnier);
            _bddContext.SaveChanges();
            return panierSaisonnier.Id;
        }

        public int ModifierPanierSaisonnier(int Id, string nomPanier, string produitsProposes, string description, decimal prix)
        {

            PanierSaisonnier panierSaisonnier = _bddContext.PanierSaisonniers.Find(Id);

            if (panierSaisonnier != null)
            {

                panierSaisonnier.Id = Id;
                panierSaisonnier.NomPanier = nomPanier;
                //panierSaisonnier.NomProducteur = nomProducteur;
                panierSaisonnier.ProduitsProposes = produitsProposes;
                panierSaisonnier.Description = description;
                panierSaisonnier.Prix = prix;
                _bddContext.SaveChanges();
            }
            return panierSaisonnier.Id;
        }

        public PanierSaisonnier ModifierPanierSaisonnier(PanierSaisonnier panierSaisonnier)
        {
            if (panierSaisonnier.Id != 0)
            {
                _bddContext.PanierSaisonniers.Update(panierSaisonnier);
                _bddContext.SaveChanges();
                return panierSaisonnier;
            }
            return null;
        }


        public void SupprimerPanierSaisonnier(int Id)
        {

            PanierSaisonnier panierSaisonnier = _bddContext.PanierSaisonniers.Find(Id);
            if (panierSaisonnier != null)
            {
                _bddContext.PanierSaisonniers.Remove(panierSaisonnier);
                _bddContext.SaveChanges();
            }
        }
    }
}
