using Projet2.Models.PanierSaisonniers;
using System;
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
        [Required(ErrorMessage = "Vous devez insérer un numero de telephone")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Le numéro doit contenir 10 chiffres")]
        public string NTelephone { get; set; }

        [Display(Name = "Adresse Mail")]
        [Required(ErrorMessage = "Le mail ne peut pas être null.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Le erter")]
        public string AdresseMail { get; set; }

        public bool EstCE { get; set; }
        public DateTime DateInscription { get; set; }


        public virtual List<LignePanierSaisonnier> CommandesPanierEffectues { get; set; }

        public ContactComiteEntreprise()
        {
            EstCE = true;
            DateInscription = DateTime.Today;

        }
        public string ReturnDateInscriptionForDisplay
        {
            get
            {
                if (this.DateInscription != null)
                {
                    return this.DateInscription.ToString("d");
                }
                return null;
            }

        }

    }
}
