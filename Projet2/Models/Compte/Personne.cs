using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projet2.Models.Compte
{
    //ajout photo profil ?
    public class Personne
    {
        public int Id { get; set; }
        public Identifiant Identifiant { get; set; }
        public int IdentifiantId { get; set; }

        [Display(Name = "Nom")]
        [Required(ErrorMessage = "Vous devez insérer un nom.")]
        public string Nom { get; set; }

        [Display(Name = "Prénom")]
        [Required(ErrorMessage = "Vous devez insérer un prénom.")]
        public string Prenom { get; set; }

        [Display(Name = "Date de Naissance")]
        [Required(ErrorMessage = "La date de naissance ne peut pas être null.")]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }

        [Display(Name = "Téléphone")]
        [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "Le numéro doit contenir 10 chiffres")]
        public int NTelephone { get; set; }


        public Adresse Adresse { get; set; }
        public int AdresseId { get; set; }


        [Display(Name = "J'accepte les conditions générales et la politique de confidentialité")]
        [Required(ErrorMessage = "Il faut accepter les conditions générales.")]
        public bool EstEnAccord { get; set; }


    }
}
