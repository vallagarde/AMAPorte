using Projet2.Models.Compte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Models.PanierSaisonniers
{
    public class PanierSaisonnier
    {
        public int Id { get; set; }
        public virtual List<Produit> ProduitsProposes { get; set; }
        public string Description { get; set; }
        public string NomProducteur { get; set; }
        public decimal Prix { get; set; }

        //public int? CataloguePanierId { get; set; }
        //public CataloguePanier CataloguePanier { get; set; }

        //public int? AdPId { get; set; }
        //public AdP AdP { get; set; }

        //public int? AdAId { get; set; }
        //public AdP AdA { get; set; }

        //public int? LignePanierSaisonnierId { get; set; }
        //public LignePanierSaisonnier  LignePanierSaisonnier { get; set; }
    }
}
