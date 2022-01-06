using System;
using System.Collections.Generic;
using Projet2.Models.Compte;

namespace Projet2.Models.Boutique
{
    public class Article
    {
        public int Id { get; set; }
        public String Nom { get; set; }
        public String Description { get; set; }
        public decimal Prix { get; set; }
        public int Stock { get; set; }
        public decimal PrixTTC { get; set; }
        public String Image { get; set; }
        public bool EstValide { get; set; }
        public bool EstEnAttente { get; set; }
        public String AdminCommentaire { get; set; }
        public DateTime DateInscription { get; set; }
        public DateTime DateModification { get; set; }


        public int AdPId { get; set; }
        public AdP AdP { get; set; }
        public virtual List<Avis> Avis { get; set; }
        public int? BoutiquesId { get; set; }
        public Boutiques Boutiques { get; set; }

        public Article()
        {
            Avis = new List<Avis>();
            AdminCommentaire = null;
            EstValide = false;
            EstEnAttente = false;
            DateInscription = DateTime.Now;
        }

        public string ReturnInscriptionDateForDisplayArticle
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

        public string ReturnModificationDateForDisplayArticle
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
