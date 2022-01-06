﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Projet2.Models.Boutique;
using Projet2.Models.Compte;
using Projet2.Models.PanierSaisonniers;
using Projet2.Models.Calendriers;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Linq;

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
        public DbSet<Commande> Commande { get; set; }

        // Calendrier:

        public DbSet<Calendrier> Calendrier { get; set; }


        //context compte
        public DbSet<AdA> AdAs { get; set; }
        public DbSet<AdP> AdPs { get; set; }
        public DbSet<Adresse> Adresses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ContactComiteEntreprise> ContactComiteEntreprises { get; set; }
        public DbSet<Entreprise> Entreprises { get; set; }
        public DbSet<Personne> Personnes { get; set; }
        public DbSet<Identifiant> Identifiants { get; set; }
        public DbSet<Admin> Admins { get; set; }

        //context panierSaisonnier
        public DbSet<PanierSaisonnier> PanierSaisonniers { get; set; }
        public DbSet<CataloguePanier> CataloguePaniers { get; set; }
        public DbSet<LignePanierSaisonnier> LignePanierSaisonniers { get; set; }
        public DbSet<CommandePanier> CommandePaniers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySql("server=localhost;user id=root;password=nitnelave;database=AmaPorte");
            if (System.Diagnostics.Debugger.IsAttached)
            {
                optionsBuilder.UseMySql("server=localhost;user id=root;password=nitnelave;database=AmaPorte");
            }
            else
            {
                IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        public void InitializeDb()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
            using (CompteServices cs = new CompteServices())
            {
                using (PanierSaisonnierService pss = new PanierSaisonnierService())
                {
                    using (LignePanierService lignePanierService = new LignePanierService())
                    {
                                    //AdAs
                                    Identifiant identifiant1 = new Identifiant()
                            {
                                AdresseMail = "ada1@gmail.com",
                                MotDePasse = "test",
                                EstAdA = true
                            };

                            int id = cs.AjouterIdentifiant(identifiant1);

                            var userClaims = new List<Claim>()
                                        {
                                            new Claim(ClaimTypes.Name, id.ToString()),
                                        };

                            var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

                            var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });

                            Personne personne1 = new Personne()
                            {
                                Identifiant = identifiant1,
                                Nom = "Dubois",
                                Prenom = "Jean Marie",
                                DateNaissance = new DateTime(1964, 05, 01),
                                NTelephone = "0234567890",
                                EstEnAccord = true,
                                EstMajeur = true

                            };
                            Adresse adresse1 = new Adresse()
                            {
                                Numero = 110,
                                Voie = "Rue de la République",
                                Ville = "Paris",
                                CodePostal = 75010,
                            };
                            AdA adA1 = new AdA()
                            {
                                EstAboAnnuel = true,
                            };
                            cs.CreerAdA(personne1, adresse1, adA1);

                            Identifiant identifiant2 = new Identifiant()
                            {
                                AdresseMail = "ada2@gmail.com",
                                MotDePasse = "test",
                                EstAdA = true
                            };

                            int id2 = cs.AjouterIdentifiant(identifiant2);

                            var userClaims2 = new List<Claim>()
                                        {
                                            new Claim(ClaimTypes.Name, id2.ToString()),
                                        };

                            var ClaimIdentity2 = new ClaimsIdentity(userClaims2, "User Identity");

                            var userPrincipal2 = new ClaimsPrincipal(new[] { ClaimIdentity2 });

                            Personne personne2 = new Personne()
                            {
                                Identifiant = identifiant2,
                                Nom = "Michel",
                                Prenom = "Héloise",
                                DateNaissance = new DateTime(1990, 06, 15),
                                NTelephone = "1234567890",
                                EstEnAccord = true,
                                EstMajeur = true

                            };
                            Adresse adresse2 = new Adresse()
                            {
                                Numero = 64,
                                Voie = "Rue des Petits Ecureuils",
                                Ville = "Levroux",
                                CodePostal = 36110,
                            };
                            AdA adA2 = new AdA()
                            {
                                EstAboAnnuel = true,
                            };
                            cs.CreerAdA(personne2, adresse2, adA2);


                            //AdPs
                            Identifiant identifiant3 = new Identifiant()
                            {
                                AdresseMail = "adp1@gmail.com",
                                MotDePasse = "test",
                                EstAdP = true
                            };

                            int id3 = cs.AjouterIdentifiant(identifiant3);

                            var userClaims3 = new List<Claim>()
                                        {
                                            new Claim(ClaimTypes.Name, id2.ToString()),
                                        };

                            var ClaimIdentity3 = new ClaimsIdentity(userClaims3, "User Identity");

                            var userPrincipal3 = new ClaimsPrincipal(new[] { ClaimIdentity3 });

                            Personne personne3 = new Personne()
                            {
                                Identifiant = identifiant3,
                                Nom = "Marchand",
                                Prenom = "Thierry",
                                DateNaissance = new DateTime(1976, 10, 01),
                                NTelephone = "1234567890",
                                EstEnAccord = true,
                                EstMajeur = true

                            };
                            Adresse adresse3 = new Adresse()
                            {
                                Numero = 15,
                                Voie = "Avenue des Fruits",
                                Ville = "Brion",
                                CodePostal = 36110,
                            };
                            AdP adP1 = new AdP()
                            {
                                Siren = 987654321,
                                NomProducteur = "Les Terres du Ruisseau",
                                Description = "L'harmonie entre l'homme et la nature.",
                                EstActive = true,
                                EstEnAttente = false,
                                EstAdP = true,
                                Vedette = true,
                                EstAboAnnuel = true,s

                            };
                            cs.CreerAdP(personne3, adresse3, adP1);

                            Identifiant identifiant4 = new Identifiant()
                            {
                                AdresseMail = "adp2@gmail.com",
                                MotDePasse = "test",
                                EstAdP = true
                            };

                            int id4 = cs.AjouterIdentifiant(identifiant4);

                            var userClaims4 = new List<Claim>()
                                        {
                                            new Claim(ClaimTypes.Name, id2.ToString()),
                                        };

                            var ClaimIdentity4 = new ClaimsIdentity(userClaims4, "User Identity");

                            var userPrincipal4 = new ClaimsPrincipal(new[] { ClaimIdentity4 });

                            Personne personne4 = new Personne()
                            {
                                Identifiant = identifiant4,
                                Nom = "Dubedout",
                                Prenom = "Julien ",
                                DateNaissance = new DateTime(1980, 02, 05),
                                NTelephone = "0234567891",
                                EstEnAccord = true,
                                EstMajeur = true

                            };
                            Adresse adresse4 = new Adresse()
                            {
                                Numero = 23,
                                Voie = "La Garenne",
                                Ville = "Levroux",
                                CodePostal = 36110,
                            };
                            AdP adP2 = new AdP()
                            {
                                Siren = 987654322,
                                NomProducteur = "La Ferme de la Haute Vallée",
                                Description = "Des Bretons, du bon produit...",
                                EstAdP = true,
                                EstActive = true,
                                EstEnAttente = false,
                                Vedette =true,
                                EstAboAnnuel = true,
                            };
                            cs.CreerAdP(personne4, adresse4, adP2);


                            //CCE
                            Identifiant identifiant5 = new Identifiant()
                            {
                                AdresseMail = "ce1@gmail.com",
                                MotDePasse = "test",
                                EstCE = true
                            };

                            int id5 = cs.AjouterIdentifiant(identifiant5);

                            var userClaims5 = new List<Claim>()
                                        {
                                            new Claim(ClaimTypes.Name, id2.ToString()),
                                        };

                            var ClaimIdentity5 = new ClaimsIdentity(userClaims5, "User Identity");

                            var userPrincipal5 = new ClaimsPrincipal(new[] { ClaimIdentity5 });

                            Entreprise entreprise1 = new Entreprise()
                            {
                                NomEntreprise = "Amazon",
                                NombreUtilisateur = 20,
                                Siren = 123456789,
                                EstEnAccord = true,
                                EstAboAnnuel = true,
                               
                            };
                            Adresse adresse5 = new Adresse()
                            {
                                Numero = 34,
                                Voie = "La Croix Chevalier",
                                Ville = "Vineuil",
                                CodePostal = 36110,
                            };

                            ContactComiteEntreprise contactComiteEntreprise1 = new ContactComiteEntreprise()
                            {
                                Identifiant = identifiant5,
                                Nom = "Duhamel",
                                Prenom = "André",
                                AdresseMail = identifiant5.AdresseMail,
                                NTelephone = "1234567890",
                                EstCE = true
                            };
                            cs.CreerCCE(contactComiteEntreprise1, entreprise1, adresse5);


                            Identifiant identifiant6 = new Identifiant()
                            {
                                AdresseMail = "ce2@gmail.com",
                                MotDePasse = "test",
                                EstCE = true
                            };

                            int id6 = cs.AjouterIdentifiant(identifiant6);

                            var userClaims6 = new List<Claim>()
                                        {
                                            new Claim(ClaimTypes.Name, id6.ToString()),
                                        };

                            var ClaimIdentity6 = new ClaimsIdentity(userClaims6, "User Identity");

                            var userPrincipal6 = new ClaimsPrincipal(new[] { ClaimIdentity6 });

                            Entreprise entreprise2 = new Entreprise()
                            {
                                NomEntreprise = "",
                                NombreUtilisateur = 20,
                                Siren = 123456789,
                                EstEnAccord = true,
                                EstAboAnnuel = true,
                            };
                            Adresse adresse6 = new Adresse()
                            {
                                Numero = 23,
                                Voie = "La Garenne",
                                Ville = "Levroux",
                                CodePostal = 36110,
                            };
                            ContactComiteEntreprise contactComiteEntreprise2 = new ContactComiteEntreprise()
                            {
                                Identifiant = identifiant6,
                                Nom = "Normand",
                                Prenom = "Noël",
                                AdresseMail = identifiant6.AdresseMail,
                                NTelephone = "1234567890",
                                EstCE = true
                            };
                            cs.CreerCCE(contactComiteEntreprise2, entreprise2, adresse6);

                            //Admin
                            Identifiant identifiant7 = new Identifiant()
                            {
                                AdresseMail = "admin@gmail.com",
                                MotDePasse = "test",
                                EstGCCQ = true
                            };

                            int id7 = cs.AjouterIdentifiant(identifiant7);

                            var userClaims7 = new List<Claim>()
                                        {
                                            new Claim(ClaimTypes.Name, id7.ToString()),
                                        };

                            var ClaimIdentity7 = new ClaimsIdentity(userClaims7, "User Identity");

                            var userPrincipal7 = new ClaimsPrincipal(new[] { ClaimIdentity7 });

                            Admin admin = new Admin()
                            {
                                Identifiant = identifiant7,
                                EstGCCQ = true
                            };
                            cs.CreerAdmin(admin);
                        

            


                        PanierSaisonnier panierSaisonnier1 = new PanierSaisonnier()
                        {

                            NomPanier = "Panier Fruits Thierry",
                            ProduitsProposes = "Pommes, Poires, Bananes, Melon",
                            Description = "Beaux fruits de saison. Mjam.",
                            Prix = 45.9m,
                            Image = "pommes.jpg",
                            AdPId = 1,
                            EstValide = true,
                            EstEnAttente = false,
                        };
                        pss.CreerPanierSaisonnier(panierSaisonnier1);

                        PanierSaisonnier panierSaisonnier2 = new PanierSaisonnier()
                        {

                            NomPanier = "Panier Legumes Thierry",
                            ProduitsProposes = "Poireaux, Carottes, Tomates, Poivrons",
                            Description = "C'est bon",
                            Prix = 45.9m,
                            Image = "onion.jpg",
                            AdPId = 1,
                            EstValide = true,
                            EstEnAttente = false,
                        };
                        pss.CreerPanierSaisonnier(panierSaisonnier2);

                        PanierSaisonnier panierSaisonnier3 = new PanierSaisonnier()
                        {

                            NomPanier = "Panier Legumes Julien",
                            ProduitsProposes = "Poireaux, Carottes, Tomates, Poivrons",
                            Description = "C'est bon",
                            Prix = 40.9m,
                            Image = "onion.jpg",
                            AdPId = 2,
                            EstValide = true,
                            EstEnAttente = false,
                            
                        };
                        pss.CreerPanierSaisonnier(panierSaisonnier3);

                        PanierSaisonnier panierSaisonnier4 = new PanierSaisonnier()
                        {

                            NomPanier = "Panier Fruits Julien",
                            ProduitsProposes = "Pommes, Poires, Bananes, Melon",
                            Description = "Poireaux, Carottes, Tomates, Poivrons",
                            Prix = 40.9m,
                            Image = "pommes.jpg",
                            AdPId = 2,
                            EstValide = true,
                            EstEnAttente = false,
                        };
                        pss.CreerPanierSaisonnier(panierSaisonnier4);


                        //Commandes Paniers

                        BddContext bddContext = new BddContext();

                        LignePanierSaisonnier lignePanierSaisonnier = lignePanierService.CreerLignePanier(1, panierSaisonnier1.Id, 1 * panierSaisonnier1.Prix, 13, adA1.Id, 0);
                        CommandePanier commandePanier = lignePanierService.CreerCommande(lignePanierSaisonnier);
                        commandePanier.EstEnLivraison = true; 
                        commandePanier.DateCommande = DateTime.Now;
                        bddContext.CommandePaniers.Update(commandePanier);

                        LignePanierSaisonnier lignePanierSaisonnier2 = lignePanierService.CreerLignePanier(1, panierSaisonnier2.Id, 1 * panierSaisonnier2.Prix, 26, adA1.Id, 0);
                        CommandePanier commandePanier2 = lignePanierService.CreerCommande(lignePanierSaisonnier2);
                        commandePanier2.EstLivre = true;
                        commandePanier2.DateCommande = DateTime.Now;
                        bddContext.CommandePaniers.Update(commandePanier2);

                        LignePanierSaisonnier lignePanierSaisonnier3 = lignePanierService.CreerLignePanier(20, panierSaisonnier1.Id, 20 * panierSaisonnier1.Prix, 13, 0, entreprise1.Id);
                        CommandePanier commandePanier3 = lignePanierService.CreerCommande(lignePanierSaisonnier3);
                        commandePanier3.EstEnPreparation = true;
                        commandePanier3.DateCommande = DateTime.Now;
                        bddContext.CommandePaniers.Update(commandePanier3);

                        LignePanierSaisonnier lignePanierSaisonnier4 = lignePanierService.CreerLignePanier(20, panierSaisonnier3.Id, 20 * panierSaisonnier3.Prix, 52, 0, entreprise1.Id);
                        CommandePanier commandePanier4 = lignePanierService.CreerCommande(lignePanierSaisonnier4);
                        commandePanier4.EstLivre = true;
                        commandePanier4.DateCommande = DateTime.Now;
                        bddContext.CommandePaniers.Update(commandePanier4);

                        bddContext.SaveChanges();

                        //Articles Boutique

                        using (ArticleRessources ar = new ArticleRessources())
                        {
                            Article article = new Article()
                            {
                                Nom = "Miel",
                                Description = "Miels de crus, pollen frais, gelée royale, propolis.",
                                Prix = 5.9m,
                                Stock = 5,
                                PrixTTC = 7.5m,
                                Image = "onion.jpg",
                                AdPId = 1
                            };
                            int a = ar.CreerArticle(article.Nom, article.Description, (int)article.Prix, article.Stock, (int)article.PrixTTC, article.Image, article.AdPId);
                            article = ar.ObtientTousLesArticles().Where(c => c.Id == a).FirstOrDefault();
                            article.EstEnAttente = false;
                            article.EstValide = true;
                            ar.ValidationArticle(article);

                            Article article2 = new Article()
                            {
                                Nom = "Confiture de Framboises",
                                Description = "Somptueuse, estivale et parfumée.",
                                Prix = 4.9m,
                                Stock = 3,
                                PrixTTC = 6.5m,
                                Image = "pommes.jpg",
                                AdPId = 1
                            };
                            int a2 = ar.CreerArticle(article2.Nom, article2.Description, (int)article2.Prix, article2.Stock, (int)article2.PrixTTC, article2.Image, article2.AdPId);
                            article2 = ar.ObtientTousLesArticles().Where(c => c.Id == a2).FirstOrDefault();
                            article2.EstEnAttente = false;
                            article2.EstValide = true;
                            ar.ValidationArticle(article2);

                            Article article3 = new Article()
                            {
                                Nom = "Miel",
                                Description = "Miels de crus, pollen frais, gelée royale, propolis.",
                                Prix = 7.9m,
                                Stock = 2,
                                PrixTTC = 9.5m,
                                Image = "onion.jpg",
                                AdPId = 1
                            };
                            int a3 = ar.CreerArticle(article3.Nom, article3.Description, (int)article3.Prix, article3.Stock, (int)article3.PrixTTC, article3.Image, article3.AdPId);
                            article3 = ar.ObtientTousLesArticles().Where(c => c.Id == a3).FirstOrDefault();
                            article3.EstEnAttente = false;
                            article3.EstValide = true;
                            ar.ValidationArticle(article3);

                            Article article4 = new Article()
                            {
                                Nom = "Confiture de Framboises",
                                Description = "Somptueuse, estivale et parfumée.",
                                Prix = 2.9m,
                                Stock = 6,
                                PrixTTC = 5.5m,
                                Image = "pommes.jpg",
                                AdPId = 1
                            };
                            int a4 = ar.CreerArticle(article4.Nom, article4.Description, (int)article4.Prix, article4.Stock, (int)article4.PrixTTC, article4.Image, article4.AdPId);
                            article4 = ar.ObtientTousLesArticles().Where(c => c.Id == a4).FirstOrDefault();
                            article4.EstEnAttente = false;
                            article4.EstValide = true;
                            ar.ValidationArticle(article4);

                            Article article5 = new Article()
                            {
                                Nom = "Miel",
                                Description = "Miels de crus, pollen frais, gelée royale, propolis.",
                                Prix = 7.9m,
                                Stock = 2,
                                PrixTTC = 9.5m,
                                Image = "onion.jpg",
                                AdPId = 2,
                            };
                            int a5 = ar.CreerArticle(article5.Nom, article5.Description, (int)article5.Prix, article5.Stock, (int)article5.PrixTTC, article5.Image, article5.AdPId);
                            article5 = ar.ObtientTousLesArticles().Where(c => c.Id == a5).FirstOrDefault();
                            article5.EstEnAttente = false;
                            article5.EstValide = true;
                            ar.ValidationArticle(article5);

                            Article article6 = new Article()
                            {
                                Nom = "Confiture de Framboises",
                                Description = "Somptueuse, estivale et parfumée.",
                                Prix = 2.9m,
                                Stock = 6,
                                PrixTTC = 5.5m,
                                Image = "pommes.jpg",
                                AdPId = 2,                  

                            };
                            int a6 = ar.CreerArticle(article6.Nom, article6.Description, (int)article6.Prix, article6.Stock, (int)article6.PrixTTC, article6.Image, article6.AdPId);
                            article6 = ar.ObtientTousLesArticles().Where(c => c.Id == a6).FirstOrDefault();
                            article6.EstEnAttente = false;
                            article6.EstValide = true;
                            ar.ValidationArticle(article6);

                            Article article7 = new Article()
                            {
                                Nom = "Confiture de Framboises",
                                Description = "Somptueuse, estivale et parfumée.",
                                Prix = 2.9m,
                                Stock = 6,
                                PrixTTC = 5.5m,
                                Image = "pommes.jpg",
                                AdPId = 1,

                            };
                            int a7 = ar.CreerArticle(article7.Nom, article7.Description, (int)article7.Prix, article7.Stock, (int)article7.PrixTTC, article7.Image, article7.AdPId);
                            article7 = ar.ObtientTousLesArticles().Where(c => c.Id == a7).FirstOrDefault();
                            article7.EstEnAttente = false;
                            article7.EstValide = true;
                            ar.ValidationArticle(article7);

                            Article article8 = new Article()
                            {
                                Nom = "Confiture de Framboises",
                                Description = "Somptueuse, estivale et parfumée.",
                                Prix = 2.9m,
                                Stock = 6,
                                PrixTTC = 5.5m,
                                Image = "pommes.jpg",
                                AdPId = 2,
                            };
                            int a8 = ar.CreerArticle(article8.Nom, article8.Description, (int)article8.Prix, article8.Stock, (int)article8.PrixTTC, article8.Image, article8.AdPId);
                            article8 = ar.ObtientTousLesArticles().Where(c => c.Id == a8).FirstOrDefault();
                            article8.EstEnAttente = false;
                            article8.EstValide = true;
                            ar.ValidationArticle(article8);


                            //Commandes Boutique

                            PanierBoutique panierBoutique1 = new PanierBoutique()
                            {
                                Total = 30m
                            };
                            this.PanierBoutique.Add(panierBoutique1);
                            this.SaveChanges();

                            PanierBoutique panierBoutique2 = new PanierBoutique()
                            {
                                //Total = LignePanierBoutique3.SousTotal + LignePanierBoutique4.SousTotal
                                Total = 30m
                            };
                            this.PanierBoutique.Add(panierBoutique2);
                            this.SaveChanges();

                            PanierBoutique panierBoutique3 = new PanierBoutique()
                            {
                                Total = 30m
                            };
                            this.PanierBoutique.Add(panierBoutique3);
                            this.SaveChanges();

                            PanierBoutique panierBoutique4 = new PanierBoutique()
                            {
                                Total = 30m
                            };
                            this.PanierBoutique.Add(panierBoutique4);
                            this.SaveChanges();


                            LignePanierBoutique LignePanierBoutique1 = new LignePanierBoutique()
                            {
                                ArticleId = article.Id,
                                Quantite = 10,
                                SousTotal = article.PrixTTC * 3,
                                PanierBoutiqueId = 1
                            };
                            this.LignePanierBoutique.Add(LignePanierBoutique1);
                            this.SaveChanges();

                            LignePanierBoutique LignePanierBoutique2 = new LignePanierBoutique()
                            {
                                ArticleId = article.Id,
                                Quantite = 1,
                                SousTotal = article.PrixTTC * 3,
                                PanierBoutiqueId = 1
                            };
                            this.LignePanierBoutique.Add(LignePanierBoutique2);
                            this.SaveChanges();

                            LignePanierBoutique LignePanierBoutique3 = new LignePanierBoutique()
                            {
                                ArticleId = article2.Id,
                                Quantite = 2,
                                SousTotal = article2.PrixTTC * 4,
                                PanierBoutiqueId = 2
                            };
                            this.LignePanierBoutique.Add(LignePanierBoutique3);
                            this.SaveChanges();

                            LignePanierBoutique LignePanierBoutique4 = new LignePanierBoutique()
                            {
                                ArticleId = article2.Id,
                                Quantite = 5,
                                SousTotal = article2.PrixTTC * 4,
                                PanierBoutiqueId = 2
                            };
                            this.LignePanierBoutique.Add(LignePanierBoutique4);
                            this.SaveChanges();

                            LignePanierBoutique LignePanierBoutique5 = new LignePanierBoutique()
                            {
                                ArticleId = article3.Id,
                                Quantite = 4,
                                SousTotal = article3.PrixTTC * 4,
                                PanierBoutiqueId = 3
                            };
                            this.LignePanierBoutique.Add(LignePanierBoutique5);
                            this.SaveChanges();

                            LignePanierBoutique LignePanierBoutique6 = new LignePanierBoutique()
                            {
                                ArticleId = article3.Id,
                                Quantite = 3,
                                SousTotal = article3.PrixTTC * 4,
                                PanierBoutiqueId = 3
                            };
                            this.LignePanierBoutique.Add(LignePanierBoutique6);
                            this.SaveChanges();

                            LignePanierBoutique LignePanierBoutique7 = new LignePanierBoutique()
                            {
                                ArticleId = article4.Id,
                                Quantite = 9,
                                SousTotal = article4.PrixTTC * 4,
                                PanierBoutiqueId = 4
                            };
                            this.LignePanierBoutique.Add(LignePanierBoutique7);
                            this.SaveChanges();

                            LignePanierBoutique LignePanierBoutique8 = new LignePanierBoutique()
                            {
                                ArticleId = article5.Id,
                                Quantite = 6,
                                SousTotal = article5.PrixTTC * 4,
                                PanierBoutiqueId = 4
                            };
                            this.LignePanierBoutique.Add(LignePanierBoutique8);
                            this.SaveChanges();


                            Client client = new Client()
                            {
                                Id = 1,
                                Nom = "Client",
                                Prenom = "Clint",
                                DateNaissance = new DateTime(1980, 02, 05),
                                NTelephone = 1234567890,
                                AdresseId = 1,
                                EstEnAccord = true
                            };
                            this.Clients.Add(client);
                            this.SaveChanges();

                            CalendrierService calendrier = new CalendrierService();

                            Commande commande1 = new Commande()
                            {
                                PanierBoutique = panierBoutique1,
                                DateCommande = DateTime.Now,
                                AdAId = 1,
                                EstEnPreparation = true,
                            };
                            this.Commande.Add(commande1);
                            this.SaveChanges();

                            Commande commande2 = new Commande()
                            {
                                PanierBoutique = panierBoutique2,
                                DateCommande = DateTime.Now,
                                AdAId = 1,
                                EstLivre = true,
                            };
                            this.Commande.Add(commande2);
                            this.SaveChanges();

                            Commande commande3 = new Commande()
                            {
                                PanierBoutique = panierBoutique3,
                                DateCommande = DateTime.Now,
                                EntrepriseId = 1,
                                EstEnLivraison = true,
                            };
                            this.Commande.Add(commande3);
                            this.SaveChanges();

                            Commande commande4 = new Commande()
                            {
                                PanierBoutique = panierBoutique4,
                                DateCommande = DateTime.Now,
                                EntrepriseId = 1,
                                EstLivre = true,
                            };
                            this.Commande.Add(commande4);
                            this.SaveChanges();

                            calendrier.AjouterLigneCalendrierCommande(commande1);
                            calendrier.AjouterLigneCalendrierCommande(commande2);
                            calendrier.AjouterLigneCalendrierCommande(commande3);
                            calendrier.AjouterLigneCalendrierCommande(commande4);
                            calendrier.AjouterLigneCalendrierPanier(commandePanier);
                            calendrier.AjouterLigneCalendrierPanier(commandePanier2);
                            calendrier.AjouterLigneCalendrierPanier(commandePanier3);
                            calendrier.AjouterLigneCalendrierPanier(commandePanier4);
                        }
                    }

                }
            }
        }
    }
}
    

