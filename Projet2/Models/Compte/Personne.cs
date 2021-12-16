using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projet2.Models.Compte
{
    //ajout photo profil ?
    public class Personne
    {
        public int Id { get; set; }

        public Identifiant identifiant { get; set; }

        [Display(Name = "Nom")]
        [Required(ErrorMessage = "Vous devez insérer un nom.")]
        public string Nom { get; set; }

        [Display(Name = "Prénom")]
        [Required(ErrorMessage = "Vous devez insérer un prénom.")]
        public string Prenom { get; set; }

        [Display(Name = "Date de Naissance")]
        [Required(ErrorMessage = "La date de naissance ne peut pas être null.")]
        [DataType(DataType.Date)]
        public DateTime? DateNaissance { get; set; }

        [Display(Name = "Adresse Mail")]
        [Required, RegularExpression(@"/^w+[+.w-]*@([w -]+.)*w+[w-]*.([a-z]{2,4}|d+)$/i", ErrorMessage = "Le mail ne peut pas être null.")] 
        public string AdresseMail { get; set; }

        [Display(Name = "Téléphone")]
        [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "Le numéro doit contenir 10 chiffres")]
        public int? NTelephone { get; set; }

        public virtual List<AdA>? AdAs { get; set; }

        public virtual List<AdP>? AdPs { get; set; }

        public virtual List<Paiement>? Paiements { get; set; }

        [Required(ErrorMessage = "Il faut accepter les conditions générales.")]
        public bool EstEnAccord { get; set; }
        public bool? EstAboAnnuel { get; set; }
        public bool? EstAboSemestre { get; set; }

    }
}
