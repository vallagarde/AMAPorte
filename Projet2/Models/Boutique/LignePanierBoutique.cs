using System;
namespace Projet2.Models
{
    public class LignePanierBoutique
    {
        public int Id { get; set; }
        public int Quantite { get; set; }
        public Article Article { get; set; }
        public int SousTotal { get; set; }


        public int PanierBoutiqueId { get; set; }
        public PanierBoutique PanierBoutique { get; set; }
    }
}
