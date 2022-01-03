using System;
using System.Collections.Generic;
using System.Linq;
using Projet2.Models.Boutique;
using Projet2.Models.PanierSaisonniers;

namespace Projet2.Models.Calendriers
{
    public class CalendrierService
    {
        private BddContext _bddContext;
        public CalendrierService()
        {
            _bddContext = new BddContext();
        }

        public List<int> ObtientLignesCalendrierPourCetteSemaine()
        {
            DateTime dateTime = DateTime.Today;

            while (dateTime.DayOfWeek != DayOfWeek.Wednesday)
            {
                dateTime = dateTime.AddDays(1);
            }

            List<Calendrier> calendriersCommandes = new List<Calendrier>();
            List<int> commandesId = new List<int>();
            calendriersCommandes = _bddContext.Calendrier.ToList();
            foreach (Calendrier calendrier in calendriersCommandes)
            {
                if(calendrier.DateLivraison.Date == dateTime.Date)
                {
                    commandesId.Add((int)calendrier.CommandeId);
                }               
            }
            return commandesId;
        }

        public void AjouterLigneCalendrierCommande(Commande commande)
        {
            DateTime dateDeCommande = commande.DateCommande;
            int commandeId = commande.Id;
            while (dateDeCommande.DayOfWeek != DayOfWeek.Wednesday)
            {
                dateDeCommande = dateDeCommande.AddDays(1);
            }
            Calendrier calendrier = new Calendrier() { CommandeId = commandeId, DateLivraison = dateDeCommande };

            _bddContext.Calendrier.Add(calendrier);
            _bddContext.SaveChanges();

        }

        public DateTime ObtenirDateLivraisonCommande(int commandeId)
        {
            var queryCalendrier = from c in _bddContext.Calendrier where c.CommandeId == commandeId select c;
            Calendrier calendrier = queryCalendrier.FirstOrDefault();
            return calendrier.DateLivraison;
        }

        public DateTime ObtenirProchaineDateDeLivraison()
        {
            DateTime dateTime = DateTime.Today;

            while (dateTime.DayOfWeek != DayOfWeek.Wednesday)
            {
                dateTime = dateTime.AddDays(1);
            }
            
            return dateTime;
        }

        /*public void AjouterLigneCalendrierPanier(PanierSaisonnierCommande panierSaisonnierCommande)
        {
            List<DateTime> liste = new List<DateTime>();
            foreach (DateTime datetime in panierSaisonnierCommande.LignePanierSaisonnierDateTimes)
            {
                DateTime dateDeCommande = panierSaisonnier.DateTime;
                liste.Add(dateCommande)
            }
            
            int commandeId = panierSaisonnier.Id;

            while (dateDeCommande.DayOfWeek != DayOfWeek.Wednesday)
            {
                dateDeCommande = dateDeCommande.AddDays(1);
            }

            Calendrier calendrier = new Calendrier() { CommandeId = commandeId, DateLivraison = dateDeCommande };

            _bddContext.Calendrier.Add(calendrier);
            _bddContext.SaveChanges();

        }*/
    }
}
