using System;
using Projet2.Models.Compte;

namespace Projet2.Models.Boutique
{
    public class Commande
    {
        public PanierBoutique PanierBoutique;
        public DateTime DateTime;

        public int AdAId { get; set; }
        public AdA AdA { get; set; }

    }
}
