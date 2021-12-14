using System;
using System.Collections.Generic;
using Projet2.Models;
using Xunit;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void Creation_Article_Verification()
        {
            // Nous supprimons la base si elle existe puis nous la créons
            using (ArticleRessources ctx = new ArticleRessources())
            {
                // Nous supprimons et créons la db avant le test
                ctx.DeleteCreateDatabase();
                // Nous créons un lieu de vacances
                ctx.CreerArticle("Champagne", "Don perignon 1000 ans d'age", 150, 200, 175);
                ctx.CreerArticle("Vin", "bordaux", 10, 5000, 12);

                ctx.ModifierArticle(2, "vin", "bordeaux", 10, 5000, 12);
                // Nous vérifions que le lieu a bien été créé

                // Nous vérifions que le lieu a bien été créé
                List<Article> articles = ctx.ObtientTousLesArticles();
                Assert.NotNull(articles);
                //Assert.Single(articles);
                Assert.Equal("Champagne", articles[0].Nom);
                Assert.Equal("Don perignon 1000 ans d'age", articles[0].Description);
            }
        }
       
    }
}
