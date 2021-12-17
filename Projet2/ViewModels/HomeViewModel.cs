using System;
using System.Collections.Generic;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;

namespace Projet2.ViewModels
{
    public class HomeViewModel
    {
        public Boutiques Boutiques = new Boutiques();
        public Article Article = new Article();

        //Pour Comptes + Authorisation
        public Personne Personne = new Personne();
        public string ReturnDateForDisplay
        {
            get
            {
                return this.Personne.DateNaissance.ToString("d");
            }
        }
        public AdA AdA = new AdA();
        public Identifiant Identifiant = new Identifiant();
        public Adresse Adresse = new Adresse();
        public AdP AdP = new AdP();
        public Entreprise Entreprise = new Entreprise();
        public ContactComiteEntreprise ContactComiteEntreprise = new ContactComiteEntreprise();
        public Admin Admin = new Admin();

        public Paiement Paiement = new Paiement();
        public bool Authentifie { get; set; }

        public LignePanierBoutique LignePanierBoutique= new LignePanierBoutique();
    }
}
