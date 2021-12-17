using System;
using System.Collections.Generic;
using Projet2.Models.Boutique;
using Projet2.Models.PanierSaisonniers;
using Xunit;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void Creation_PanierSaisonnier_Verification()
        {
            // Nous supprimons la base si elle existe puis nous la créons
            using (PanierSaisonnierService ctx = new PanierSaisonnierService())
            {
                // Nous supprimons et créons la db avant le test
                ctx.DeleteCreateDatabase();
                // Nous créons un lieu de vacances
                ctx.CreerPanierSaisonnier(new List<Produit> { new Produit() { Id = 1, NomProduit = "Pomme" }, new Produit() { Id = 2, NomProduit = "Orange" }, new Produit() { Id = 3, NomProduit = "Avocat" }, new Produit() { Id = 4, NomProduit = "Raisin" } }, "Produits de saisons", "Jean Charles", 17.450M);
                //ctx.CreerPanierSaisonnier(new List<string>(){ "30 oeufs", "poulets"}, "Elévés en plein air", "Louis Ferrand", 10.50M);
                //ctx.CreerPanierSaisonnier(new List<string>(){ "Haut de dinde", "Poirreaux", "Tommates", "Poivrons" }, "Bio", "Jeanne Dasilva", 15.90M);
                //ctx.CreerPanierSaisonnier(new List<string>(){ "Potiron", "Pomme de terre", "Melon" }, "Bio", "Richard Laurant", 16.90M);

                // int id = ctx.ModifierPanierSaisonnier({ "Pomme", "Orange", "Avocat", "Raisin" }, "Produits de saisons", "Jean Charles", 17.450M);


                // Nous vérifions que le lieu a bien été créé
                //List<PanierSaisonnier> panierSaisonniers = ctx.ObtientTousLesPaniers();
                //Assert.NotNull(panierSaisonniers);
                //Assert.Single(articles);
                //CollectionAssert.AreEqual
                //List<Produit> Expected = new List<Produit> { new Produit() { Id = 1, NomProduit = "Pomme" }, new Produit() { Id = 2, NomProduit = "Orange" }, new Produit() { Id = 3, NomProduit = "Avocat" }, new Produit() { Id = 4, NomProduit = "Raisin" } };
                //var toto = (panierSaisonniers[0].ProduitsProposes).GetType();
                //Assert.Equal(Expected, panierSaisonniers[0].ProduitsProposes);
                //Assert.Equal("Produits de saisons", panierSaisonniers[0].Description);
                //Assert.Equal("Jean Charles", panierSaisonniers[0].NomProducteur);
                //Assert.Equal(17.450M, panierSaisonniers[0].Prix);

                ////Assert.Equal(new List<string>() {"30 oeufs", "poulets"}, panierSaisonniers[0].ProduitsProposes);
                //Assert.Equal("Elévés en plein air", panierSaisonniers[0].Description);
                //Assert.Equal("Louis Ferrand", panierSaisonniers[0].NomProducteur);
                //Assert.Equal(10.50M, panierSaisonniers[0].Prix);

                //Assert.Equal(new List<string>() { "Haut de dinde", "Poirreaux", "Tommates", "Poivrons" }, panierSaisonniers[0].ProduitsProposes);
                //Assert.Equal("Bio", panierSaisonniers[0].Description);
                //Assert.Equal("Jeanne Dasilva", panierSaisonniers[0].NomProducteur);
                //Assert.Equal(15.90M, panierSaisonniers[0].Prix);

                //Assert.Equal(new List<string>() { "Potiron", "Pomme de terre", "Melon" }, panierSaisonniers[0].ProduitsProposes);
                //Assert.Equal("Bio", panierSaisonniers[0].Description);
                //Assert.Equal("Richard Laurant", panierSaisonniers[0].NomProducteur);
                //Assert.Equal(16.90M, panierSaisonniers[0].Prix);

                //ctx.SupprimerPanierSaisonnier(4);
            }
        }

    }
}
