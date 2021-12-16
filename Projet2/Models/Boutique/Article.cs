using System;
using System.Collections.Generic;

namespace Projet2.Models.Boutique
{
    public class Article
    {
        public int Id { get; set; }
        public String Nom { get; set; }
        public String Description { get; set; }
        public decimal Prix { get; set; }
        public int Stock { get; set; }
        public decimal PrixTTC { get; set; }

        public int? AssortimentId { get; set; }
        public Assortiment assortiment { get; set; }

        public virtual List<Avis> Avis { get; set; }

        public int? LignePanierBoutiqueId { get; set; }
        public LignePanierBoutique LignePanierBoutique { get; set; }

        public int? BoutiquesId { get; set; }
        public Boutiques Boutiques { get; set; }


    }
}
