using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projet2.Models.Compte
{
    public class Personne
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vous devez insérer un nom.")]
        [Display(Name = "Nom")]
        public string? Nom { get; set; }

        [Required(ErrorMessage = "Vous devez insérer un prénom.")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "La date de naissance ne peut pas être null.")]
        [Display(Name = "Date de Naissance")]
        [DataType(DataType.Date)]
        public DateTime? DateNaissance { get; set; }

        [Required(ErrorMessage = "Le mail ne peut pas être null.")]
        [Display(Name = "Adresse Mail")]
        public string? AdresseMail { get; set; }

        [Display(Name = "Téléphone")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Le numéro doit contenir 10 chiffres")]
        public int? NTelephone { get; set; }

        public string? MotDePasse { get; set; }


        public virtual List<AdA>? AdAs { get; set; }

        public virtual List<AdP>? AdPs { get; set; }

        public virtual List<Paiement>? Paiements { get; set; }

        [Required(ErrorMessage = "Il faut accepter les conditions générales.")]
        public bool? EstEnAccord { get; set; }
        public bool? EstAboAnnuel { get; set; }
        public bool? EstAboSemestre { get; set; }

    }
}
