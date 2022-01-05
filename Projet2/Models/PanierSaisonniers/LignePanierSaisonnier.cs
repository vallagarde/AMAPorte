using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Models.PanierSaisonniers
{
    public class LignePanierSaisonnier
    {
        public int Id { get; set; }
        public int Quantite { get; set; }
        public int DureeAbonnement { get; set; }

        public int PanierSaisonnierId { get; set; }
        public PanierSaisonnier PanierSaisonnier { get; set; }

        public decimal SousTotal { get; set; }

        //public int? CommandePanierId { get; set; }
        //public CommandePanier CommandePanier { get; set; }

        public Avis Avis { get; set; }
        public int? AvisId { get; set; }

        public int? AdAId { get; set; }
        public AdA AdA { get; set; }

        public int? EntrepriseId { get; set; }
        public Entreprise Entreprise { get; set; }
    }
}
