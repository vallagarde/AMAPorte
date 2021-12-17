using System.Collections.Generic;

namespace Projet2.Models.Compte
{
    public class AdA
    {
        public int Id { get; set; }

        public bool EstAdA { get; set; }

        public Personne Personne { get; set; }
        public int PersonneId { get; set; }

        public bool EstAboAnnuel { get; set; }
        public bool EstAboSemestre { get; set; }

        public Paiement Paiement { get; set; }
        public int? PaiementId { get; set; }

        //public List<AdP> ProducteursFavoris { get; set; }

        //public List<Atelier> AteliersFavoris { get; set; }

        //public List<Article> ArticlesFavoris { get; set; }

        //public List<Commande> CommandesBoutiqueEffectues { get; set; }

        //public List<?> CommandesPanierEffectues { get; set; }
        public AdA()
        {
            EstAdA = true;
        }

    }
}
