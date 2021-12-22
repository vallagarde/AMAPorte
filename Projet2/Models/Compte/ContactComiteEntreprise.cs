using Projet2.Models.PanierSaisonniers;
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

        [Display(Name = "Nom")]
        [Required(ErrorMessage = "Vous devez insérer un nom.")]
        public string Nom { get; set; }

        [Display(Name = "Prénom")]
        [Required(ErrorMessage = "Vous devez insérer un prénom.")]
        public string Prenom { get; set; }

        [Display(Name = "Téléphone")]
        [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "Le numéro doit contenir 10 chiffres")]
        public int NTelephone { get; set; }

        public string AdresseMail { get; set; }

        public bool EstCE { get; set; }

        public virtual List<LignePanierSaisonnier> CommandesPanierEffectues { get; set; }

        public ContactComiteEntreprise()
        {
            EstCE = true;
        }

    }
}
