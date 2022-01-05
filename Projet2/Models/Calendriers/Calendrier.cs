using System;
using Projet2.Models.Boutique;
using Projet2.Models.PanierSaisonniers;

namespace Projet2.Models.Calendriers
{
    public class Calendrier
    {
        public int Id { get; set; }
        public DateTime DateLivraison { get; set; }

        public Commande Commande { get; set; }
        public int? CommandeId { get; set; }

        public PanierSaisonnier PanierSaisonnier { get; set; }
        public int? PanierSaisonnierId { get; set; }

    }
}
