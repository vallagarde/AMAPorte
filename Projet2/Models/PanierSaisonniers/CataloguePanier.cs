using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Models.PanierSaisonniers
{
    public class CataloguePanier
    {
        public int Id { get; set; }
        public String NomCatalogue { get; set; }
        public int NombrePaniers { get; set; }
        public virtual List<PanierSaisonnier> PanierSaisonniers { get; set; }

    }
}
