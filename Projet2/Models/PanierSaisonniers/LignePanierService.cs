using Microsoft.EntityFrameworkCore;
using Projet2.Models.Compte;
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
        public void Dispose()
        {
            _bddContext.Dispose();
        }
        public LignePanierSaisonnier ObtientLignePanierParId(int id)
        {
            LignePanierSaisonnier panier = _bddContext.LignePanierSaisonniers.Where(c => c.Id == id).FirstOrDefault();
            return panier;
        }
        public List<LignePanierSaisonnier> ObtientLignePanierParAdA(int id)
        {
            List<LignePanierSaisonnier> panier = _bddContext.LignePanierSaisonniers.Where(c => c.AdA.Id == id).ToList();
            return panier;
        }

        public PanierSaisonnier ObtientPanierParId(int id)
        {
            PanierSaisonnier panier = _bddContext.PanierSaisonniers.Where(c => c.Id == id).FirstOrDefault();
            return panier;
        }

        public LignePanierSaisonnier CreerLignePanier(int quantite, int PanierSaisonnierId, decimal sousTotal, int semaine, int AdAId, int EntrepriseId)
        {
            LignePanierSaisonnier lignePanier = new LignePanierSaisonnier();

            if (AdAId != 0)
            {
                lignePanier = new LignePanierSaisonnier() { PanierSaisonnierId = PanierSaisonnierId, Quantite = quantite, SousTotal = sousTotal*semaine, AdAId = AdAId, DureeAbonnement = semaine };

            }
            else
            {
                lignePanier = new LignePanierSaisonnier() { PanierSaisonnierId = PanierSaisonnierId, Quantite = quantite, SousTotal = sousTotal * semaine, EntrepriseId = EntrepriseId, DureeAbonnement = semaine };

            }

            _bddContext.LignePanierSaisonniers.Add(lignePanier);
            _bddContext.SaveChanges();
            return lignePanier;
        }

        public CommandePanier CreerCommande(LignePanierSaisonnier lignePanier)
        {
            PanierSaisonnier panier = ObtientPanierParId(lignePanier.PanierSaisonnierId);
            CommandePanier commande = new CommandePanier() { LignePanierSaisonnier = lignePanier, PanierSaisonnier = panier, DateCommande = DateTime.Today, Total = lignePanier.SousTotal, AdAId = lignePanier.AdAId, EntrepriseId = lignePanier.EntrepriseId , EstEnPreparation = true};
            _bddContext.CommandePaniers.Add(commande);
            _bddContext.SaveChanges();
            return commande;
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

        public List<CommandePanier> ObtenirToutesCommandesPanier()
        {
            List<CommandePanier> commandePanierSaisonniers = _bddContext.CommandePaniers.ToList();
            foreach (CommandePanier commandePanierSaisonnier in commandePanierSaisonniers)
            {
                commandePanierSaisonnier.PanierSaisonnier = (from panier in _bddContext.PanierSaisonniers where panier.Id == commandePanierSaisonnier.PanierSaisonnierId select panier).FirstOrDefault();
                commandePanierSaisonnier.PanierSaisonnier.AdP = (from adp in _bddContext.AdPs where adp.Id == commandePanierSaisonnier.PanierSaisonnier.AdPId select adp).FirstOrDefault();
                commandePanierSaisonnier.PanierSaisonnier.AdP.Personne = (from p in _bddContext.Personnes where p.Id == commandePanierSaisonnier.PanierSaisonnier.AdP.PersonneId select p).FirstOrDefault();
                commandePanierSaisonnier.LignePanierSaisonnier = (from ligne in _bddContext.LignePanierSaisonniers where ligne.Id == commandePanierSaisonnier.LignePanierSaisonnierId select ligne).FirstOrDefault();
                commandePanierSaisonnier.PanierSaisonnier.AdP.Personne.Adresse = (from a in _bddContext.Adresses where a.Id == commandePanierSaisonnier.PanierSaisonnier.AdP.Personne.AdresseId select a).FirstOrDefault();
                commandePanierSaisonnier.LignePanierSaisonnier.Avis = (from avis in _bddContext.Avis where avis.Id == commandePanierSaisonnier.LignePanierSaisonnier.AvisId select avis).FirstOrDefault();

                commandePanierSaisonnier.AdA = (from ada in _bddContext.AdAs where ada.Id == commandePanierSaisonnier.AdAId select ada).FirstOrDefault();
                if (commandePanierSaisonnier.AdA != null)
                {
                    commandePanierSaisonnier.AdA.Personne = (from personne in _bddContext.Personnes where personne.Id == commandePanierSaisonnier.AdA.PersonneId select personne).FirstOrDefault();
                    commandePanierSaisonnier.AdA.Personne.Adresse = (from adresse in _bddContext.Adresses where adresse.Id == commandePanierSaisonnier.AdA.Personne.AdresseId select adresse).FirstOrDefault();
                }

                commandePanierSaisonnier.Entreprise = (from entreprise in _bddContext.Entreprises where entreprise.Id == commandePanierSaisonnier.EntrepriseId select entreprise).FirstOrDefault();
                if (commandePanierSaisonnier.Entreprise != null)
                {
                    commandePanierSaisonnier.Entreprise.Adresse = (from adresse in _bddContext.Adresses where adresse.Id == commandePanierSaisonnier.Entreprise.AdresseId select adresse).FirstOrDefault();

                    commandePanierSaisonnier.PanierSaisonnier = (from panier in _bddContext.PanierSaisonniers where panier.Id == commandePanierSaisonnier.LignePanierSaisonnier.PanierSaisonnierId select panier).FirstOrDefault();
                }
            }
            return commandePanierSaisonniers;
        }

        public List<CommandePanier> ObtenirCommandesPanierAdP(int adpId)
        {
            List<CommandePanier> commandePanierSaisonniers = new List<CommandePanier>();
            AdP adp = (from a in _bddContext.AdPs where a.Id == adpId select a).FirstOrDefault();

            List<PanierSaisonnier> panierSaisonniers = (from panier in _bddContext.PanierSaisonniers where panier.AdPId == adpId select panier).ToList();
            foreach (PanierSaisonnier panierSaisonnier in panierSaisonniers)
            {
                List<CommandePanier> commandePaniers = (from c in _bddContext.CommandePaniers where c.PanierSaisonnierId == panierSaisonnier.Id select c).ToList();
                foreach (CommandePanier commandePanierSaisonnier in commandePaniers)
                {
                    commandePanierSaisonnier.PanierSaisonnier = panierSaisonnier;
                    commandePanierSaisonnier.PanierSaisonnier.AdP = adp;
                    commandePanierSaisonnier.LignePanierSaisonnier = (from ligne in _bddContext.LignePanierSaisonniers where ligne.Id == commandePanierSaisonnier.LignePanierSaisonnierId select ligne).FirstOrDefault();
                    commandePanierSaisonnier.LignePanierSaisonnier.Avis = (from avis in _bddContext.Avis where avis.Id == commandePanierSaisonnier.LignePanierSaisonnier.AvisId select avis).FirstOrDefault();

                    commandePanierSaisonnier.AdA = (from ada in _bddContext.AdAs where ada.Id == commandePanierSaisonnier.AdAId select ada).FirstOrDefault();
                    if (commandePanierSaisonnier.AdA != null)
                    {
                        commandePanierSaisonnier.AdA.Personne = (from personne in _bddContext.Personnes where personne.Id == commandePanierSaisonnier.AdA.PersonneId select personne).FirstOrDefault();
                        commandePanierSaisonnier.AdA.Personne.Adresse = (from adresse in _bddContext.Adresses where adresse.Id == commandePanierSaisonnier.AdA.Personne.AdresseId select adresse).FirstOrDefault();
                    }
                    commandePanierSaisonnier.Entreprise = (from entreprise in _bddContext.Entreprises where entreprise.Id == commandePanierSaisonnier.EntrepriseId select entreprise).FirstOrDefault();
                    if (commandePanierSaisonnier.Entreprise != null)
                    {
                        commandePanierSaisonnier.Entreprise.Adresse = (from adresse in _bddContext.Adresses where adresse.Id == commandePanierSaisonnier.Entreprise.AdresseId select adresse).FirstOrDefault();
                    }
                    bool alreadyExist = commandePanierSaisonniers.Contains(commandePanierSaisonnier);
                    if (!alreadyExist) commandePanierSaisonniers.Add(commandePanierSaisonnier);
                }
            }
            return commandePanierSaisonniers;
        }


        public List<CommandePanier> ObtenirCommandesPanierParAdA(AdA ada)
        {
            List<CommandePanier> commandesAdA = (from c in _bddContext.CommandePaniers where c.AdAId == ada.Id select c).ToList();
            foreach (CommandePanier commandePanier in commandesAdA)
            {
                commandePanier.LignePanierSaisonnier = (from l in _bddContext.LignePanierSaisonniers where l.Id == commandePanier.LignePanierSaisonnierId select l).FirstOrDefault();
                commandePanier.PanierSaisonnier = (from p in _bddContext.PanierSaisonniers where p.Id == commandePanier.PanierSaisonnierId select p).FirstOrDefault();
                commandePanier.PanierSaisonnier.AdP = (from a in _bddContext.AdPs where a.Id == commandePanier.PanierSaisonnier.AdPId select a).FirstOrDefault();
                commandePanier.PanierSaisonnier.AdP.Personne = (from p in _bddContext.Personnes where p.Id == commandePanier.PanierSaisonnier.AdP.PersonneId select p).FirstOrDefault();
                commandePanier.PanierSaisonnier.AdP.Personne.Adresse = (from a in _bddContext.Adresses where a.Id == commandePanier.PanierSaisonnier.AdP.Personne.AdresseId select a).FirstOrDefault();
                commandePanier.LignePanierSaisonnier.Avis = (from avis in _bddContext.Avis where avis.Id == commandePanier.LignePanierSaisonnier.AvisId select avis).FirstOrDefault();
                ada.CommandesPanierEffectues.Add(commandePanier);
            }
            return ada.CommandesPanierEffectues;
        }

        public List<CommandePanier> ObtenirCommandesPanierParEntreprise(Entreprise entreprise)
        {
            List<CommandePanier> commandesEntreprise = (from c in _bddContext.CommandePaniers where c.EntrepriseId == entreprise.Id select c).ToList();
            foreach (CommandePanier commandePanier in commandesEntreprise)
            {
                commandePanier.LignePanierSaisonnier = (from l in _bddContext.LignePanierSaisonniers where l.Id == commandePanier.LignePanierSaisonnierId select l).FirstOrDefault();
                commandePanier.PanierSaisonnier = (from p in _bddContext.PanierSaisonniers where p.Id == commandePanier.LignePanierSaisonnier.PanierSaisonnierId select p).FirstOrDefault();
                commandePanier.PanierSaisonnier.AdP = (from a in _bddContext.AdPs where a.Id == commandePanier.PanierSaisonnier.AdPId select a).FirstOrDefault();
                commandePanier.PanierSaisonnier.AdP.Personne = (from p in _bddContext.Personnes where p.Id == commandePanier.PanierSaisonnier.AdP.PersonneId select p).FirstOrDefault();
                commandePanier.PanierSaisonnier.AdP.Personne.Adresse = (from a in _bddContext.Adresses where a.Id == commandePanier.PanierSaisonnier.AdP.Personne.AdresseId select a).FirstOrDefault();
                commandePanier.LignePanierSaisonnier.Avis = (from avis in _bddContext.Avis where avis.Id == commandePanier.LignePanierSaisonnier.AvisId select avis).FirstOrDefault();
                entreprise.CommandesPanierEffectues.Add(commandePanier);
            }
            return entreprise.CommandesPanierEffectues;
        }

        public void ChangerEtatCommandePanier(int panierId, string etat)
        {
            CommandePanier commandePanier = (from c in _bddContext.CommandePaniers where c.Id == panierId select c).FirstOrDefault();
            commandePanier.EtatCommande = "";
            commandePanier.EstEnPreparation = false;
            commandePanier.EstEnLivraison = false;
            commandePanier.EstARecuperer = false;
            commandePanier.EstLivre = false;
            commandePanier.EtatCommande = etat;
            _bddContext.Update(commandePanier);
            _bddContext.SaveChanges();
        }


    }
}
