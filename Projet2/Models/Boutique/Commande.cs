using System;
using Projet2.Models.Compte;

namespace Projet2.Models.Boutique
{
    public class Commande
    {
        public int Id { get; set; }
        public PanierBoutique PanierBoutique { get; set; }
        public DateTime DateTime { get; set; }

        public int? AdAId { get; set; }
        public AdA AdA { get; set; }

        public int? EntrepriseId { get; set; }
        public Entreprise Entreprise { get; set; }

        public int? ClientId { get; set; }
        public Client Client { get; set; }

    }
}
