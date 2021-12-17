using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projet2.Models.Compte
{
    public class ContactComiteEntreprise
    {
        public int Id { get; set; }

        public Identifiant Identifiant { get; set; }
        public int IdentifiantId { get; set; }

        public Entreprise Entreprise { get; set; }
        public int EntrepriseId { get; set; }

        [Display(Name = "Téléphone")]
        [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "Le numéro doit contenir 10 chiffres")]
        public int NTelephone { get; set; }

        public bool EstAboAnnuel { get; set; }
        public bool EstAboSemestre { get; set; }

        public static bool EstCE { get; set; }

        public Paiement Paiement { get; set; }
        public int? PaiementId { get; set; }

        //public virtual List<AdP> ProducteursFavoris { get; set; }

        //public virtual List<Atelier> AteliersFavoris { get; set; }

        //public virtual List<Article> ArticlesFavoris { get; set; }

        //public virtual List<Commande> CommandesBoutiqueEffectues { get; set; }

        //public virtual List<?> CommandesPanierEffectues { get; set; }

        public ContactComiteEntreprise()
        {
            EstCE = true;
        }

    }
}
