using System;
using System.Collections.Generic;

namespace Projet2.Models
{
    public class Boutiques
    {
        public int Id { get; set; }
        public int NombreArticle { get; set; }
        public virtual List<Article> Articles { get; set; }

    }
}
