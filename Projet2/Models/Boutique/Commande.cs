using System;
using Projet2.Models.Compte;

namespace Projet2.Models.Boutique
{
    public class Commande
    {
        public int Id { get; set; }
        public PanierBoutique PanierBoutique { get; set; }
        public int PanierBoutiqueId { get; set; }
        public DateTime DateTime { get; set; }

        public int? AdAId { get; set; }
        public AdA AdA { get; set; }

        public int? EntrepriseId { get; set; }
        public Entreprise Entreprise { get; set; }

        public int? ClientId { get; set; }
        public Client Client { get; set; }


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

    }
}
