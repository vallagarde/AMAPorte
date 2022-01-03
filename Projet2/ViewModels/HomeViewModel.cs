using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Projet2.Helpers;
using Projet2.Models;
using Projet2.Models.Boutique;
using Projet2.Models.Calendriers;
using Projet2.Models.Compte;
using Projet2.Models.PanierSaisonniers;

namespace Projet2.ViewModels
{
    public class HomeViewModel
    {
        public Boutiques Boutiques = new Boutiques();
        public Article Article = new Article();
        public Upload Upload = new Upload();
        public int PanierId { get; set; }
        public PanierBoutique PanierBoutique = new PanierBoutique();
        public Commande Commande = new Commande();
        public Avis Avis = new Avis();



        //Paniers
        public PanierSaisonnier PanierSaisonnier = new PanierSaisonnier();
        public CataloguePanier CataloguePanier = new CataloguePanier();
        public LignePanierSaisonnier LignePanierSaisonnier = new LignePanierSaisonnier();

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
        public Client Client = new Client();
        public List<AdA> ListeComptesAdA = new List<AdA>();
        public List<AdP> ListeComptesAdP = new List<AdP>();
        public List<ContactComiteEntreprise> ListeComptesCCEs = new List<ContactComiteEntreprise>();
        public List<Commande> ListeCommandesEnPrep = new List<Commande>();
        public List<Commande> ListeCommandesEnCours = new List<Commande>();
        public List<Commande> ListeCommandesLivres = new List<Commande>();
        public List<Commande> ListeCommandesARecup = new List<Commande>();
        public List<LignePanierBoutique> ListeCommandesAPrep = new List<LignePanierBoutique>();
        public List<LignePanierBoutique> ListeCommandesDonnees = new List<LignePanierBoutique>();
        public DateTime ProchaineDateLivraison = new DateTime();
        public bool AdresseExistante;

        public string ReturnDateForDisplayLivraison
        {
            get
            {
                if (this.ProchaineDateLivraison != null)
                {
                    return this.ProchaineDateLivraison.ToString("d");
                }
                return null;
            }
        }

        public bool Authentifie { get; set; }

        public LignePanierBoutique LignePanierBoutique= new LignePanierBoutique();
    }
}
