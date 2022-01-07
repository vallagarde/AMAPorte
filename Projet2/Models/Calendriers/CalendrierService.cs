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
            Calendrier calendrier = new Calendrier() { CommandeId = commandeId, DateLivraison = dateDeCommande.Date };

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
            DateTime dateTime = new DateTime().ToLocalTime();

            while (dateTime.DayOfWeek != DayOfWeek.Wednesday)
            {
                dateTime = dateTime.AddDays(1);
            }
            
            return dateTime;
        }

        public void AjouterLigneCalendrierPanier(CommandePanier commandePanier)
        {
            DateTime dateDeCommande = commandePanier.DateCommande;
            int commandeId = commandePanier.Id;
            while (dateDeCommande.DayOfWeek != DayOfWeek.Wednesday)
            {
                dateDeCommande = dateDeCommande.AddDays(1);
            }
            int nbSemaine = commandePanier.LignePanierSaisonnier.DureeAbonnement;

            Calendrier calendrier = new Calendrier() { LignePanierSaisonnierId = commandeId , DateLivraison = dateDeCommande.Date };
            _bddContext.Calendrier.Add(calendrier);

            for (int i =0; i< nbSemaine; i++)
            {
                dateDeCommande = dateDeCommande.AddDays(7);
                Calendrier calendrier2 = new Calendrier() { LignePanierSaisonnierId = commandeId, DateLivraison = dateDeCommande.Date };
                _bddContext.Calendrier.Add(calendrier2);
            }

            _bddContext.SaveChanges();

        }
        public List<DateTime> ObtenirDatesLivraisonPanierSaisonnier(int commandeLignePanierSaisonnierId)
        {
            List<Calendrier> listeCalendrier = _bddContext.Calendrier.Where(c => c.LignePanierSaisonnierId == commandeLignePanierSaisonnierId).ToList();
            List<DateTime> listeDatedeLivraison = new List<DateTime>();

            foreach (Calendrier cal in listeCalendrier)
            {
                listeDatedeLivraison.Add(cal.DateLivraison);
            }

            return listeDatedeLivraison;
        }
    }
}
