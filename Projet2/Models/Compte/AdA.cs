using System.Collections.Generic;

namespace Projet2.Models.Compte
{
    public class AdA
    {
        public int Id { get; set; }
        public virtual List<ContactComiteEntreprise>? ContactComiteEntreprises { get; set; }
    }
}
