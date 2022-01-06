using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projet2.Models.Compte
{
    //ajout photo profil ?
    public class Personne
    {
        public int Id { get; set; }

        [Display(Name = "Nom")]
        [Required, RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Vous devez insérer un nom.")]
        public string Nom { get; set; }

        [Display(Name = "Prénom")]
        [Required, RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Vous devez insérer un prénom.")]
        public string Prenom { get; set; }

        [Display(Name = "Date de Naissance")]
        [Required(ErrorMessage = "Vous devez insérer une date de naissance")]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }

        [Display(Name = "Téléphone")]
        [Required(ErrorMessage = "Vous devez insérer un numero de telephone")]
        [RegularExpression(@"\d{10}", ErrorMessage = "Le numéro doit contenir 10 chiffres")]
        public string NTelephone { get; set; }

        [Display(Name = "J’atteste que j’ai lu et accepté les conditions générales de vente de AMAPorte.")]
        [Required(ErrorMessage = "Il faut accepter les conditions générales.")]
        public bool EstEnAccord { get; set; }        
        public bool EstMajeur { get; set; }
        public Identifiant Identifiant { get; set; }
        public int IdentifiantId { get; set; }
        public Adresse Adresse { get; set; }
        public int AdresseId { get; set; }

        public int getAge()
        {
            var today = DateTime.Today;
            var birthDate = this.DateNaissance;
            var age = today.Year - birthDate.Year;
            var m = today.Month - birthDate.Month;
            var d = today.Day - birthDate.Day;
            if (m < 0 || (m == 0 && d < 0))
            {
                age--;
            }
            return age;
        }


    }
}
