﻿using System;
namespace Projet2.Models.Boutique
{
    public class LignePanierBoutique
    { 

        public int Id { get; set; }
        public int Quantite { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }


        public decimal SousTotal { get; set; }

        public Avis Avis { get; set; }
        public int? AvisId { get; set; }

        public int PanierBoutiqueId { get; set; }
        public PanierBoutique PanierBoutique { get; set; }
    }
}
