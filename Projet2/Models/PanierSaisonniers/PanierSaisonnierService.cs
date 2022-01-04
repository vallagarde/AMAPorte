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

        public PanierSaisonnier ObtientPanierParId(int id)
        {
            PanierSaisonnier panier = _bddContext.PanierSaisonniers.Where(c => c.Id == id).FirstOrDefault();
          
                //var queryAdP = from adp in _bddContext.AdPs where adp.Id == panier.AdPId select adp;
                //panier.AdP = queryAdP.First();
            return panier;
        }
        public LignePanierSaisonnier ObtientLignePanierParId(int id)
        {
            LignePanierSaisonnier panier = _bddContext.LignePanierSaisonniers.Where(c => c.Id == id).FirstOrDefault();

            //var queryAdP = from adp in _bddContext.AdPs where adp.Id == panier.AdPId select adp;
            //panier.AdP = queryAdP.First();
            return panier;
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
                panierSaisonnier.AdP = adp;
                adp.AssortimentPanier.Add(panierSaisonnier);
            }
            return adp.AssortimentPanier;
        }

        //public int CreerPanierSaisonnier(string nomPanier, string produitsProposes, string description, decimal prix, decimal prixTTC, string imagePanier, int adpId)
        //{
        //    PanierSaisonnier panierSaisonnier = new PanierSaisonnier() { NomPanier = nomPanier,  ProduitsProposes = produitsProposes, Description = description, Prix = prix, PrixTTC = prixTTC, Image = imagePanier, AdPId = adpId};
        //    _bddContext.PanierSaisonniers.Add(panierSaisonnier);
        //    _bddContext.SaveChanges();
        //    return panierSaisonnier.Id;
        //}
        public int CreerPanierSaisonnier(string nomPanier, string produitsProposes, string description, decimal prix, string imagePanier, int adpId)
        {
            PanierSaisonnier panierSaisonnier = new PanierSaisonnier() { NomPanier = nomPanier, ProduitsProposes = produitsProposes, Description = description, Prix = prix, Image = imagePanier, AdPId = adpId };
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

        public void CreerCommande(LignePanierSaisonnier lignePanier)
        {
            PanierSaisonnier panier = ObtientPanierParId(lignePanier.PanierSaisonnierId);
            CommandePanier commande = new CommandePanier() { LignePanierSaisonnier = lignePanier, PanierSaisonnier = panier, DateTime = DateTime.Today, AdAId = 1, ContactComiteEntrepriseId = 1, total = lignePanier.SousTotal*lignePanier.DureeAbonnement };
            _bddContext.CommandePaniers.Add(commande);
            _bddContext.SaveChanges();
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
