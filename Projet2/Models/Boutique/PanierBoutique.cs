using System;
using System.Collections.Generic;

namespace Projet2.Models.Boutique
{
    public class PanierBoutique
    {
        public int Id { get; set; }

        public virtual List<LignePanierBoutique> LignePanierBoutiques{ get; set; }

        public decimal Total { get; set; }
    }
}
