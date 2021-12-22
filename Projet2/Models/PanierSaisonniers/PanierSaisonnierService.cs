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
            List<PanierSaisonnier> listePaniers = _bddContext.PanierSaisonniers.ToList();
            foreach (PanierSaisonnier panierSaisonnier in listePaniers)
            {
                var queryAdP = from adp in _bddContext.AdPs where adp.Id == panierSaisonnier.AdPId select adp;
                panierSaisonnier.AdP = queryAdP.First();
            }
            //return _bddContext.PaniersSaisonniers.Include(p => p.ProduitsProposes).ToList();
            return listePaniers;
        }
        //public List<Produit> ObtientTousLesProduits()
        //{
        //    return _bddContext.Produits.ToList();
        //}x

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
                panierSaisonnier.AdP = adp;
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
            //List<Produit> produits = ObtientTousLesProduits;
            if (panierSaisonnier != null)
            {
                _bddContext.PanierSaisonniers.Remove(panierSaisonnier);
                //SupprimerProduitPropose(panierSaisonnier.ProduitsProposes);
                _bddContext.SaveChanges();
            }

        }

        //public void SupprimerProduitPropose (List<Produit> list)
        //{
        //    Produit produitpropose = _bddContext.Produits.Find();
        //    if (produitpropose != null)
        //    {
        //        _bddContext.Produits.Remove(produitpropose);
        //        _bddContext.SaveChanges();
        //    }
        //}
        //public void ChercherProduit(string id)
        //{
        //    Produit produit = _bddContext().Produits.ToList();
        //    Personne personneASupprimer = _bddContext.Personnes.Find(adaASupprimer.PersonneId);

        //    if (produit != null)
        //    {
        //        _bddContext.Produits.Remove(produit);
        //        _bddContext.SaveChanges();
        //    }
        //}
    }
}
