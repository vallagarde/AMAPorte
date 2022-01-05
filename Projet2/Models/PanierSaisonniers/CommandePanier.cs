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
        public DateTime DateTime { get; set; }

        public int PanierSaisonnierId { get; set; }
        public PanierSaisonnier PanierSaisonnier { get; set; }

        public int LignePanierSaisonnierId { get; set; }
        public LignePanierSaisonnier LignePanierSaisonnier { get; set; }

        public decimal total { get; set; }

        public int? AdAId { get; set; }
        public AdA AdA { get; set; }

        public int? ContactComiteEntrepriseId { get; set; }
        public ContactComiteEntreprise ContactComiteEntreprise { get; set; }

    }
}
