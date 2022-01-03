using System;
using System.Collections.Generic;

namespace Projet2.Models.Boutique
{
    public interface IPanierService : IDisposable
    {
        List<PanierBoutique> ObtientTousLesPaniers();
        List<LignePanierBoutique> ObtientTousLesLignes();

        int CreerLigne(int quantite, int ArticleId, decimal sousTotal, int panierId);
        int ModifierLigneRelatif(int id ,int quantite, Article article, decimal sousTotal, int panierId);
        int CreerPanier();
        void AjouterArticle(int PanierId, int ArticleId, int quantite);
        int ModifierPanier(int id ,List<LignePanierBoutique> liste);
    }
}
