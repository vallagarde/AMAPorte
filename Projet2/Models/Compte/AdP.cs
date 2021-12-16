using Projet2.Models.Boutique;

namespace Projet2.Models.Compte
{

    //plusieurs photos pour presentation ?
    //Consulter son tableau de bord > attributs manquants ? a voir
    public class AdP
    {
        public int Id { get; set; }
        public int Siren { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = " ";
        public Assortiment AssortimentBoutique { get; set; }
        public int AssortimentId { get; set; }
        //public List<?> PanierDuMoment { get; set; }

        public Personne Personne { get; set; }
        public int PersonneId { get; set; }




    }
}
