using Projet2.Models.Compte;
using System;
using System.Collections.Generic;

namespace Projet2.Models.Boutique
{
    public interface IArticleRessources : IDisposable
    {
        void DeleteCreateDatabase();
        List<Article> ObtientTousLesArticles();

        List<Article> ObtenirArticleParAdP(AdP adp);
        int CreerArticle(string nom, string description, int prix, int stock, int prixTTC, String imageNom, int adpId);

        int ModifierArticle(int id, string nom, string description, decimal prix, int stock, decimal prixTTC, int adpId);
        void SupprimerArticle(int Id);


    }
}
