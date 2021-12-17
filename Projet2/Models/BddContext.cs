using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using Projet2.Models.PanierSaisonniers;

namespace Projet2.Models
{
    public class BddContext : DbContext
    {
        //context boutique
        public DbSet<Article> Article { get; set; }
        public DbSet<Avis> Avis { get; set; }
        public DbSet<Boutiques> Boutiques { get; set; }
        public DbSet<LignePanierBoutique> LignePanierBoutique { get; set; }
        public DbSet<PanierBoutique> PanierBoutique { get; set; }

        //context compte
        public DbSet<AdA> AdAs { get; set; }
        public DbSet<AdP> AdPs { get; set; }
        public DbSet<Adresse> Adresses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ContactComiteEntreprise> ContactComiteEntreprises { get; set; }
        public DbSet<Entreprise> Entreprises { get; set; }
        public DbSet<Personne> Personnes { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<Identifiant> Identifiants { get; set; }
        public DbSet<Role> Roles { get; set; }

        //context panierSaisonnier
        public DbSet<Produit> Produits { get; set; }
        public DbSet<PanierSaisonnier> PaniersSaisonniers { get; set; }
        public DbSet<CataloguePanier> CataloguesPaniers { get; set; }
        public DbSet<LignePanierSaisonnier> LignePaniersSaisonniers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user id=root;password=nitnelave;database=AmaPorte");
        }

        public void InitializeDb()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();

        }
    }
}
