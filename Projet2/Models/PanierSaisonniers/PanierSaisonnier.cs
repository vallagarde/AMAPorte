using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Models.PanierSaisonniers
{
    public class PanierSaisonnier
    {
        public int Id { get; set; }
        [Display(Name = "Nom du panier")]
        public string NomPanier { get; set; }

        [Display(Name = "Produits propsés")]
        public virtual string ProduitsProposes { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        CultureInfo ci_fr = new CultureInfo("fr-from");
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Prix { get; set; }

        public decimal PrixTTC { get; set; }

        public String Image { get; set; }

        [Display(Name = "J'accepte les conditions générales et la politique de confidentialité")]
        [Required(ErrorMessage = "Il faut accepter les conditions générales.")]
        public bool AccordContrat { get; set; }

        public int AdPId { get; set; }
        public AdP AdP { get; set; }

        public bool EstValide { get; set; }
        public bool EstEnAttente { get; set; }
        public string AdminCommentaire { get; set; }

        public DateTime DateInscription { get; set; }
        public DateTime DateModification { get; set; }

        public virtual List<Avis> Avis { get; set; }

        //public int? LignePanierSaisonnierId { get; set; }
        //public LignePanierSaisonnier  LignePanierSaisonnier { get; set; }

        public PanierSaisonnier()
        {
            Avis = new List<Avis>();
            EstValide = false;
            EstEnAttente = false;
            AdminCommentaire = null;
            DateInscription = DateTime.Now;
        }

        public string ReturnInscriptionDateForDisplayPanier
        {
            get
            {
                if (this.DateInscription != DateTime.MinValue)
                {
                    return this.DateInscription.ToString("d");
                }
                return null;
            }
        }

        public string ReturnModificationDateForDisplayPanier
        {
            get
            {
                if (this.DateModification != DateTime.MinValue)
                {
                    return this.DateModification.ToString("d");
                }
                return null;
            }
        }
    }
}
