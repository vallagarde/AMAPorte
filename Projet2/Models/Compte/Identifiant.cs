using System.ComponentModel.DataAnnotations;

namespace Projet2.Models.Compte
{
    public class Identifiant
    {
        public int Id { get; set; }
        public string MotDePasse { get; set; }

        [Display(Name = "Adresse Mail")]
        [Required(ErrorMessage = "Le mail ne peut pas être null.")]
        public string AdresseMail { get; set; }

    }
}
