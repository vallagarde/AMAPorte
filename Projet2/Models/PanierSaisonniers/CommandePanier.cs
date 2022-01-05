using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Models.PanierSaisonniers
{
    public class CommandePanier
    {

        public int Id { get; set; }
        public DateTime DateCommande { get; set; }
        public DateTime DateLivraison { get; set; }

        public int PanierSaisonnierId { get; set; }
        public PanierSaisonnier PanierSaisonnier { get; set; }

        public int LignePanierSaisonnierId { get; set; }
        public LignePanierSaisonnier LignePanierSaisonnier { get; set; }

        public decimal Total { get; set; }

        public int? AdAId { get; set; }
        public AdA AdA { get; set; }

        public int? EntrepriseId { get; set; }
        public Entreprise Entreprise { get; set; }



        public bool EstEnPreparation { get; set; }

        public bool EstEnLivraison { get; set; }

        public bool EstARecuperer { get; set; }

        public bool EstLivre { get; set; }
        public string EtatCommande
        {
            get
            {
                if (EstEnPreparation) return "EstEnPreparation";
                else if (EstEnLivraison) return "EstEnLivraison";
                else if (EstARecuperer) return "EstARecuperer";
                else if (EstLivre) return "EstLivre";
                return null;
            }
            set
            {
                switch (value)
                {
                    case "EstEnPreparation":
                        EstEnPreparation = true;
                        break;
                    case "EstEnLivraison":
                        EstEnLivraison = true;
                        break;
                    case "EstARecuperer":
                        EstARecuperer = true;
                        break;
                    case "EstLivre":
                        EstLivre = true;
                        break;
                }
            }
        }

        public string ReturnDateLivraisonForDisplayCommande
        {
            get
            {
                if (this.DateLivraison != null)
                {
                    return this.DateLivraison.ToString("d");
                }
                return null;
            }
        }

        public string ReturnDateCommandeForDisplayCommande
        {
            get
            {
                if (this.DateCommande != null)
                {
                    return this.DateCommande.ToString("d");
                }
                return null;
            }
        }

    }
}
