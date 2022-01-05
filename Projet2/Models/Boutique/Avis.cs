using Projet2.Models.Compte;
using Projet2.Models.PanierSaisonniers;
using System;
namespace Projet2.Models.Boutique
{
    public class Avis
    {
        public int Id { get; set; }
        public String Text { get; set; }
        public int Note { get; set; }

        public int? ArticleId { get; set; }
        public Article Article { get; set; }

        public int? PanierSaisonnierId { get; set; }
        public PanierSaisonnier PanierSaisonnier { get; set; }


        public AdA AdA { get; set; }
        public int? AdAId { get; set; }

        public Entreprise Entreprise { get; set; }
        public int? EntrepriseId { get; set; }

    }
}
