using System.Collections.Generic;

namespace Projet2.Models.Compte
{
    public class AdA
    {
        public int Id { get; set; }
        public virtual List<ContactComiteEntreprise>? ContactComiteEntreprises { get; set; }

        public List<AdP> ProducteursFavoris { get; set; }

        //public List<Atelier> AteliersFavoris { get; set; }

        //public List<Article> ArticlesFavoris { get; set; }

        //public List<Commande> CommandesBoutiqueEffectues { get; set; }

        //public List<?> CommandesPanierEffectues { get; set; }

    }
}
