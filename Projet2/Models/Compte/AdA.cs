using Projet2.Models.Boutique;
using Projet2.Models.PanierSaisonniers;
using System;
using System.Collections.Generic;


namespace Projet2.Models.Compte
{
    public class AdA
    {
        public int Id { get; set; }

        public bool EstAdA { get; set; }
        public DateTime DateInscription { get; set; }

        public string Image { get; set; }

        public Personne Personne { get; set; }
        public int PersonneId { get; set; }

        public bool EstAboAnnuel { get; set; }
        public bool EstAboSemestre { get; set; }

        public virtual List<AdP> ProducteursFavoris { get; set; }

        //public List<Atelier> AteliersFavoris { get; set; }

        public virtual List<Article> ArticlesFavoris { get; set; }

        public virtual List<Commande> CommandesBoutiqueEffectues { get; set; }

        public virtual List<CommandePanier> CommandesPanierEffectues { get; set; }

        public AdA()
        {
            EstAdA = true;
            DateInscription = DateTime.Today;
            Image = "Default.jpg";
            CommandesBoutiqueEffectues = new List<Commande>();
            CommandesPanierEffectues = new List<CommandePanier>();
            ArticlesFavoris = new List<Article>();
            ProducteursFavoris = new List<AdP>();
            //AteliersFavoris = new List<Atelier>();
        }

    }
}
