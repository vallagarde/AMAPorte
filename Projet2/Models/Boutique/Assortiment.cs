using System;
using System.Collections.Generic;
using Projet2.Models.Compte;

namespace Projet2.Models.Boutique
{
    public class Assortiment
    {
        public int Id { get; set; }

        public List<Article> Articles { get; set; }

    }
}
