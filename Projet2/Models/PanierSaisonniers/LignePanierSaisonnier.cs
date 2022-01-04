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

        public int? ContactComiteEntrepriseId { get; set; }
        public ContactComiteEntreprise ContactComiteEntreprise { get; set; }

    }
}
