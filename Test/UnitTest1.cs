using Projet2.Models;
using Projet2.Models.Compte;
using Projet2.Models.Boutique;
using Projet2.Models.PanierSaisonniers;
using System.Collections.Generic;
using Xunit;
using System;
using System.Security.Claims;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void Creation_CataloguePaniers_Verification()
        {

            using (PanierSaisonnierService pss = new PanierSaisonnierService())
            {

                PanierSaisonnier panierSaisonnier1 = new PanierSaisonnier()
                {

                    NomPanier = "Panier Fruits Thierry",
                    ProduitsProposes = "Pommes, Poires, Bananes, Melon",
                    Description = "Beaux fruits de saison. Mjam.",
                    Prix = 45.9m,
                    Image = "pommes.jpg",
                    AdPId = 1
                };
                pss.CreerPanierSaisonnier(panierSaisonnier1);

                PanierSaisonnier panierSaisonnier2 = new PanierSaisonnier()
                {

                    NomPanier = "Panier Legumes Thierry",
                    ProduitsProposes = "Poireaux, Carottes, Tomates, Poivrons",
                    Description = "C'est bon",
                    Prix = 45.9m,
                    Image = "onion.jpg",
                    AdPId = 1
                };
                pss.CreerPanierSaisonnier(panierSaisonnier2);

                PanierSaisonnier panierSaisonnier3 = new PanierSaisonnier()
                {

                    NomPanier = "Panier Legumes Julien",
                    ProduitsProposes = "Poireaux, Carottes, Tomates, Poivrons",
                    Description = "C'est bon",
                    Prix = 40.9m,
                    Image = "onion.jpg",
                    AdPId = 2
                };
                pss.CreerPanierSaisonnier(panierSaisonnier3);

                PanierSaisonnier panierSaisonnier4 = new PanierSaisonnier()
                {

                    NomPanier = "Panier Fruits Julien",
                    ProduitsProposes = "Pommes, Poires, Bananes, Melon",
                    Description = "Poireaux, Carottes, Tomates, Poivrons",
                    Prix = 40.9m,
                    Image = "pommes.jpg",
                    AdPId = 2
                };
                pss.CreerPanierSaisonnier(panierSaisonnier4);

            }
        }


        [Fact]
        public void Creation_Boutique_Verification()
        {

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
                ar.CreerArticle(article.Nom, article.Description, (int)article.Prix, article.Stock, (int)article.PrixTTC, article.Image, article.AdPId);

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
                ar.CreerArticle(article2.Nom, article2.Description, (int)article2.Prix, article2.Stock, (int)article2.PrixTTC, article2.Image, article2.AdPId);

                Article article3 = new Article()
                {
                    Nom = "Miel",
                    Description = "Miels de crus, pollen frais, gelée royale, propolis.",
                    Prix = 7.9m,
                    Stock = 2,
                    PrixTTC = 9.5m,
                    Image = "onion.jpg",
                    AdPId = 2
                };
                ar.CreerArticle(article3.Nom, article3.Description, (int)article3.Prix, article3.Stock, (int)article3.PrixTTC, article3.Image, article3.AdPId);

                Article article4 = new Article()
                {
                    Nom = "Confiture de Framboises",
                    Description = "Somptueuse, estivale et parfumée.",
                    Prix = 2.9m,
                    Stock = 6,
                    PrixTTC = 5.5m,
                    Image = "pommes.jpg",
                    AdPId = 2
                };
                ar.CreerArticle(article4.Nom, article4.Description, (int)article4.Prix, article4.Stock, (int)article4.PrixTTC, article4.Image, article4.AdPId);

                Article article5 = new Article()
                {
                    Nom = "Confiture de Framboises",
                    Description = "Somptueuse, estivale et parfumée.",
                    Prix = 2.9m,
                    Stock = 6,
                    PrixTTC = 5.5m,
                    Image = "pommes.jpg",
                    AdPId = 2
                };
                ar.CreerArticle(article5.Nom, article5.Description, (int)article5.Prix, article5.Stock, (int)article5.PrixTTC, article5.Image, article5.AdPId);

                Article article6 = new Article()
                {
                    Nom = "Confiture de Framboises",
                    Description = "Somptueuse, estivale et parfumée.",
                    Prix = 2.9m,
                    Stock = 6,
                    PrixTTC = 5.5m,
                    Image = "pommes.jpg",
                    AdPId = 2
                };
                ar.CreerArticle(article6.Nom, article6.Description, (int)article6.Prix, article6.Stock, (int)article6.PrixTTC, article6.Image, article6.AdPId);

            }
        }


        [Fact]
        public void Creation_Comptes_PourBaseDeDonnees()
        {
            using (CompteServices cs = new CompteServices())
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
                    NTelephone = 1234567890,
                    EstEnAccord = true,
                    EstMajeur = true

                };
                Adresse adresse1 = new Adresse()
                {
                    Numero = 110,
                    Voie = "Rue de la République",
                    Ville = "Paris",
                    CodePostal = 75010,
                    Pays = "France"
                };
                AdA adA1 = new AdA();
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
                    NTelephone = 1234567890,
                    EstEnAccord = true,
                    EstMajeur = true

                };
                Adresse adresse2 = new Adresse()
                {
                    Numero = 64,
                    Voie = "Rue des Petits Ecureuils",
                    Ville = "Levroux",
                    CodePostal = 36110,
                    Pays = "France"
                };
                AdA adA2 = new AdA();
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
                    NTelephone = 1234567890,
                    EstEnAccord = true,
                    EstMajeur = true

                };
                Adresse adresse3 = new Adresse()
                {
                    Numero = 15,
                    Voie = "Avenue des Fruits",
                    Ville = "Brion",
                    CodePostal = 36110,
                    Pays = "France"
                };
                AdP adP1 = new AdP()
                {
                    Siren = 987654321,
                    NomProducteur = "Les Terres du Ruisseau",
                    Description = "L'harmonie entre l'homme et la nature.",
                    EstAdP = true
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
                    NTelephone = 1234567890,
                    EstEnAccord = true,
                    EstMajeur = true

                };
                Adresse adresse4 = new Adresse()
                {
                    Numero = 23,
                    Voie = "La Garenne",
                    Ville = "Levroux",
                    CodePostal = 36110,
                    Pays = "France"
                };
                AdP adP2 = new AdP()
                {
                    Siren = 987654322,
                    NomProducteur = "La Ferme de la Haute Vallée",
                    Description = "Des Bretons, du bon produit...",
                    EstAdP = true
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
                    EstEnAccord = true
                };
                Adresse adresse5 = new Adresse()
                {
                    Numero = 34,
                    Voie = "La Croix Chevalier",
                    Ville = "Vineuil",
                    CodePostal = 36110,
                    Pays = "France"
                };

                ContactComiteEntreprise contactComiteEntreprise1 = new ContactComiteEntreprise()
                {
                    Identifiant = identifiant5,
                    Nom = "Duhamel",
                    Prenom = "André",
                    AdresseMail = identifiant5.AdresseMail,
                    NTelephone = 1234567890,
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
                    EstEnAccord = true
                };
                Adresse adresse6 = new Adresse()
                {
                    Numero = 23,
                    Voie = "La Garenne",
                    Ville = "Levroux",
                    CodePostal = 36110,
                    Pays = "France"
                };
                ContactComiteEntreprise contactComiteEntreprise2 = new ContactComiteEntreprise()
                {
                    Identifiant = identifiant6,
                    Nom = "Normand",
                    Prenom = "Noël",
                    AdresseMail = identifiant5.AdresseMail,
                    NTelephone = 1234567890,
                    EstCE = true
                };
                cs.CreerCCE(contactComiteEntreprise2, entreprise2, adresse6);

                //CCE
                Identifiant identifiant7 = new Identifiant()
                {
                    AdresseMail = "admin@gmail.com",
                    MotDePasse = "test",
                    EstCE = true
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
            }
        }







    }
}
