using System;
using Microsoft.EntityFrameworkCore;
using Projet2.Models.Boutique;

namespace Projet2.Models
{
    public class BddContext : DbContext
    {
        public DbSet<Article> Article { get; set; }
        public DbSet<Avis> Avis { get; set; }
        public DbSet<Boutiques> Boutiques { get; set; }
        public DbSet<LignePanierBoutique> LignePanierBoutique { get; set; }
        public DbSet<PanierBoutique> PanierBoutique { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user id=root;password=nitnelave;database=testouille");
        }
    }
}
