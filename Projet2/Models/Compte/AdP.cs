using Projet2.Models.Boutique;
using Projet2.Models.PanierSaisonniers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Projet2.Models.Compte
{

    //plusieurs photos pour presentation ?
    //Consulter son tableau de bord > attributs manquants ? a voir
    public class AdP
    {
        public int Id { get; set; }

        [Display(Name = "Numéro SIREN")]
        [Required(ErrorMessage = "Vous devez insérer un numéro SIREN.")]
        public int Siren { get; set; }

        [Display(Name = "Nom de la Production")]
        [Required(ErrorMessage = "Vous devez insérer un nom.")]
        public string NomProducteur { get; set; }

        [Display(Name = "Ecrivez quelques mot sur votre production")]
        public string Description { get; set; }

        public virtual List<Article> Assortiment { get; set; }

        public virtual List<PanierSaisonnier> AssortimentPanier { get; set; }

        public bool EstAdP { get; set; }

        public Personne Personne { get; set; }
        public int PersonneId { get; set; }

        public AdP()
        {
            EstAdP = true;
            Assortiment = new List<Article>();
            AssortimentPanier = new List<PanierSaisonnier>();
        }


    }
}
