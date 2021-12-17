using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Models.PanierSaisonniers
{
    public class Produit
    {
        public int Id { get; set; }
        public string NomProduit { get; set; }

        public int PanierSaisonnierId { get; set; }
        public PanierSaisonnier PanierSaisonnier { get; set; }


    }
}
