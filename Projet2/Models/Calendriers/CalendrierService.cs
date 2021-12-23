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

        public List<Calendrier> ObtientLignesCalendrierPourCetteSemaine()
        {
            DateTime dateTime = DateTime.Now;

            while (dateTime.DayOfWeek != DayOfWeek.Wednesday)
            {
                dateTime = dateTime.AddDays(1);

            }

            List<Calendrier> commandesId = new List<Calendrier>();
            commandesId = _bddContext.Calendrier.Where(a => a.DateLivraison.DayOfWeek == dateTime.DayOfWeek).ToList();
            return commandesId;
        }

        public void AjouterLigneCalendrierCommande(Commande commande)
        {
            DateTime dateDeCommande = commande.DateTime;
            int commandeId = commande.Id;
            while (dateDeCommande.DayOfWeek != DayOfWeek.Wednesday)
            {
                dateDeCommande = dateDeCommande.AddDays(1);
            }
            Calendrier calendrier = new Calendrier() { CommandeId = commandeId, DateLivraison = dateDeCommande };

            _bddContext.Calendrier.Add(calendrier);
            _bddContext.SaveChanges();

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
