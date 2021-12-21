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
        public DbSet<Admin> Admins { get; set; }

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
            this.PaniersSaisonniers.AddRange(

            new PanierSaisonnier { 
                Id = 1, 
                //ProduitsProposes =  new List<Produit> { new Produit() { NomProduit = "Pomme" }, new Produit() { NomProduit = "Orange" }, new Produit() { NomProduit = "Avocat" }, new Produit() { NomProduit = "Raisin" } },
                NomPanier = "Panier de fruits",
                ProduitsProposes = "Pomme,Orange,Avocat, Raisin",
                Description = "Produits de saisons", 
                NomProducteur = "Jean Charles",
                Image = "",
                Prix = 10.450M },
            new PanierSaisonnier{Id = 2, NomPanier = "Panier d'oeufs", ProduitsProposes = "20 oeufs", Description = "Origine poules elévées en plein air", NomProducteur = "Louis Ferrand", Prix = 3.50M, Image = "eggs.jpg" },
            new PanierSaisonnier{Id = 3, NomPanier = "Panier mixte", ProduitsProposes = "Haut de dinde, poulet fermier, Poirreaux, Tommates, Poivrons, Carotte", Description = "Bio", NomProducteur = "Jeanne Dasilva", Prix = 15.90M, Image = "vegetables.jpg" },
            new PanierSaisonnier{Id = 4, NomPanier = "Panier ensoleillé", ProduitsProposes = "Potiron, Pomme de terre, Melon", Description = "Bio", NomProducteur = "Richard Laurant", Prix = 14.50M, Image = "pumpkins.jpg" }
            );

            this.SaveChanges();
        }
    }
}
