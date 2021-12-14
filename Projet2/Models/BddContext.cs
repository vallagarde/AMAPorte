﻿using System;
using Microsoft.EntityFrameworkCore;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user id=root;password=nitnelave;database=AmaPorte");
        }

        public void InitializeDb()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
            this.Personnes.AddRange(
                new Personne
                {
                    Id = 1,
                    Nom = "Bernard",
                    Prenom = "Hugo",
                    DateNaissance = new System.DateTime(1984, 09, 28),
                    AdresseMail = "hugobernard@mail.com",
                    NTelephone = 1111111111,
                },
                new Personne
                {
                    Id = 2,
                    Nom = "Levi",
                    Prenom = "Alexandre",
                    DateNaissance = new System.DateTime(1990, 09, 10),
                    AdresseMail = "alexandrelevi@mail.com",
                    NTelephone = 1111111111,
                }
            );

            this.SaveChanges();
        }
    }
}
