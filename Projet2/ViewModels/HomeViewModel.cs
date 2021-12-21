using System;
using System.Collections.Generic;
using Projet2.Models;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using Projet2.Models.PanierSaisonniers;

namespace Projet2.ViewModels
{
    public class HomeViewModel
    {
        public Boutiques Boutiques = new Boutiques();
        public Article Article = new Article();
        public Upload Upload = new Upload();

        //Paniers
        public PanierSaisonnier PanierSaisonnier = new PanierSaisonnier();

        //Pour Comptes + Authorisation
        public Personne Personne = new Personne();
        public string ReturnDateForDisplay
        {
            get
            {

                if (this.Personne != null) 
                {
                    return this.Personne.DateNaissance.ToString("d");
                }
                return null;
            }
        }
        public AdA AdA = new AdA();
        public Identifiant Identifiant = new Identifiant();
        public Adresse Adresse = new Adresse();
        public AdP AdP = new AdP();
        public Entreprise Entreprise = new Entreprise();
        public ContactComiteEntreprise ContactComiteEntreprise = new ContactComiteEntreprise();
        public Admin Admin = new Admin();
        public List<AdA> ListeComptesAdA = new List<AdA>();
        public List<AdP> ListeComptesAdP = new List<AdP>();
        public List<ContactComiteEntreprise> ListeComptesCCEs = new List<ContactComiteEntreprise>();

        public Paiement Paiement = new Paiement();
        public bool Authentifie { get; set; }

        public LignePanierBoutique LignePanierBoutique= new LignePanierBoutique();
    }
}
