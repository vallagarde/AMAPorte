using System;
namespace Projet2.Models
{
    public class Avis
    {
        public int Id { get; set; }
        public String Text { get; set; }
        public int Note { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
