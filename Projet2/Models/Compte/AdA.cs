using System.Collections.Generic;

namespace Projet2.Models
{
    public class AdA
    {
        public int Id { get; set; }
        public virtual List<ContactComiteEntreprise>? ContactComiteEntreprises { get; set; }
    }
}
