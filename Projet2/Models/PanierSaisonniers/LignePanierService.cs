using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Models.PanierSaisonniers
{
    public class LignePanierService : ILignePanierSercice
    {
        private BddContext _bddContext;

        public LignePanierService()
        {
            _bddContext = new BddContext();
        }

        public LignePanierSaisonnier ObtientLignePanier(int Id)
        {
            return _bddContext.LignePanierSaisonniers.Include(c => c.PanierSaisonnierId).Where(c => c.Id == Id).FirstOrDefault();
        }

        public LignePanierSaisonnier lignePanierSaisonnier(int Id)
        {
            return _bddContext.LignePanierSaisonniers.Find(Id);
        }

        public PanierSaisonnier ObtientPanierSaisonnier(int Id)
        {
            return _bddContext.PanierSaisonniers.Find(Id);
        }

        public LignePanierSaisonnier CreerLignePanier(int quantite, int PanierSaisonnierId, decimal sousTotal, int semaine)
        {
            LignePanierSaisonnier lignePanier = new LignePanierSaisonnier() { PanierSaisonnierId = PanierSaisonnierId, Quantite = quantite, SousTotal = sousTotal, ContactComiteEntrepriseId = 1, DureeAbonnement = semaine };

            _bddContext.LignePanierSaisonniers.Add(lignePanier);
            _bddContext.SaveChanges();
            return lignePanier;
        }

        //public void AjouterLignePanier()
        //{
        //    PanierSaisonnierService ctxpanier = new PanierSaisonnierService();
        //    PanierSaisonnier panier = ctxpanier.ObtientTousLesPaniers().Where(p => p.Id == PanierSaisonnierId).FirstOrDefault();
        //}
        

        public void CreerCommandeLigne()
        {
            CommandePanier commande = new CommandePanier() { };
            _bddContext.CommandePaniers.Add(commande);
            _bddContext.SaveChanges();
        }

        public CommandePanier ObtientCommande (int commandeId)
        {
            return _bddContext.CommandePaniers.Include(c => c.LignePanierSaisonnier).ThenInclude(c => c.PanierSaisonnier).Where(c => c.Id == commandeId).FirstOrDefault();
        }

        public void AjouterLigne()
        {

        }

        public int ModifierLignePanier(int id, int quantite, PanierSaisonnier panierSaisonnier, decimal sousTotal)
        {
            LignePanierSaisonnier lignePanier = _bddContext.LignePanierSaisonniers.Find(id);

            if (lignePanier != null)
            {

                lignePanier.Id = id;
                lignePanier.Quantite += quantite;
                lignePanier.SousTotal += sousTotal;

                _bddContext.SaveChanges();
            }

            return lignePanier.Id;
        }


        public void CalculerTotalCommandeLigne(int id)
        {
            CommandePanier commande = _bddContext.CommandePaniers.Find(id);
            LignePanierSaisonnier ligne = _bddContext.LignePanierSaisonniers.Find(commande.LignePanierSaisonnier);
            //commande.Total = 0;
          
            //commande.Total += ligne.SousTotal;
           
            _bddContext.SaveChanges();
        }

        public void CalculerTotalCommandePanier(int id)
        {
            CommandePanier commande = _bddContext.CommandePaniers.Find(id);
            PanierSaisonnier panier = _bddContext.PanierSaisonniers.Find(commande.PanierSaisonnier);
            //commande.Total = 0;

            //commande.Total += panier;

            _bddContext.SaveChanges();
        }

        public void Dispose()
        {
            _bddContext.Dispose();
        }
    }
}
