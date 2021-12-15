using System.ComponentModel.DataAnnotations;

namespace Projet2.Models.Compte
{
    public class Identifiant
    {
        public int Id { get; set; }
        public string MotDePasse { get; set; }


        [Display(Name = "Adresse Mail")]
        [Required, RegularExpression(@"/^w+[+.w-]*@([w -]+.)*w+[w-]*.([a-z]{2,4}|d+)$/i", ErrorMessage = "Le mail ne peut pas être null.")]
        public string AdresseMail { get; set; }

    }
}
