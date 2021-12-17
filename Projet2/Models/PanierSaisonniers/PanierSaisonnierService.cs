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
            return _bddContext.PaniersSaisonniers.ToList();
        }

        public void Dispose()
        {
            _bddContext.Dispose();
        }

        public int CreerPanierSaisonnier(List<Produit> produitsProposes, string description, string nomProducteur, decimal prix)
        {
            PanierSaisonnier panierSaisonnier = new PanierSaisonnier() { ProduitsProposes = produitsProposes, Description = description, NomProducteur = nomProducteur, Prix = prix};
            _bddContext.PaniersSaisonniers.Add(panierSaisonnier);
            _bddContext.SaveChanges();
            return panierSaisonnier.Id;
        }

        public int CreerPanierSaisonnier(PanierSaisonnier panierSaisonnier)
        {
            _bddContext.PaniersSaisonniers.Add(panierSaisonnier);
            _bddContext.SaveChanges();
            return panierSaisonnier.Id;
        }

        public int ModifierPanierSaisonnier(int Id, List<Produit> produitsProposes, string description, string nomProducteur, decimal prix)
        {

            PanierSaisonnier panierSaisonnier = _bddContext.PaniersSaisonniers.Find(Id);

            if (panierSaisonnier != null)
            {

                panierSaisonnier.Id = Id;
 
                panierSaisonnier.ProduitsProposes = produitsProposes;
                panierSaisonnier.Description = description;
                panierSaisonnier.NomProducteur = nomProducteur;
                panierSaisonnier.Prix = prix;
                _bddContext.SaveChanges();
            }

            return panierSaisonnier.Id;
        }

        public int ModifierPanierSaisonnier(PanierSaisonnier panierSaisonnier)
        {

            if (panierSaisonnier != null)
            {

                _bddContext.PaniersSaisonniers.Update(panierSaisonnier);
                _bddContext.SaveChanges();
            }

            return panierSaisonnier.Id;
        }


        public void SupprimerPanierSaisonnier(int Id)
        {

            PanierSaisonnier panierSaisonnier = _bddContext.PaniersSaisonniers.Find(Id);
            if (panierSaisonnier != null)
            {
                _bddContext.PaniersSaisonniers.Remove(panierSaisonnier);
                _bddContext.SaveChanges();
            }

        }
    }
}
