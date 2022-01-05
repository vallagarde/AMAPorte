using Projet2.Models.Boutique;
using Projet2.Models.PanierSaisonniers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Projet2.Models.Compte
{
    public class Entreprise
    {
        public int Id { get; set; }

        [Display(Name = "Collaborateurs")]
        [Required(ErrorMessage = "Vous devez insérer un nombre.")]
        public int NombreUtilisateur { get; set; }

        [Display(Name = "Nom")]
        [Required(ErrorMessage = "Vous devez insérer un nom.")]
        public string NomEntreprise { get; set; }

        [Display(Name = "SIREN")]
        [Required(ErrorMessage = "Vous devez insérer un numéro SIREN.")]
        public int Siren { get; set; }
        public Adresse Adresse { get; set; }
        public int AdresseId { get; set; }


        public virtual List<ContactComiteEntreprise> ListeContact {get; set;}

        public bool EstAboAnnuel { get; set; }
        public bool EstAboSemestre { get; set; }

        public virtual List<AdP> ProducteursFavoris { get; set; }

        //public List<Atelier> AteliersFavoris { get; set; }

        public virtual List<Article> ArticlesFavoris { get; set; }

        public virtual List<Commande> CommandesBoutiqueEffectues { get; set; }

        public virtual List<CommandePanier> CommandesPanierEffectues { get; set; }


        [Display(Name = "J'accepte les conditions générales et la politique de confidentialité")]
        [Required(ErrorMessage = "Il faut accepter les conditions générales.")]
        public bool EstEnAccord { get; set; }

        public Entreprise()
        {
            CommandesBoutiqueEffectues = new List<Commande>();
            CommandesPanierEffectues = new List<CommandePanier>();
            ArticlesFavoris = new List<Article>();
            ProducteursFavoris = new List<AdP>();
            //AteliersFavoris = new List<Atelier>();
        }


    }
}
