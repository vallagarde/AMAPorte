using System;
using System.Collections.Generic;

namespace Projet2.Models.Boutique
{
    public interface IPanierService : IDisposable
    {
        List<PanierBoutique> ObtientTousLesPaniers();
        List<LignePanierBoutique> ObtientTousLesLignes();

        int CreerLigne(int quantite, Article article, decimal sousTotal);
        int ModifierLigne(int id ,int quantite, Article article, decimal sousTotal);
        int CreerPanier(List<LignePanierBoutique> liste);
        int ModifierPanier(int id ,List<LignePanierBoutique> liste);
    }
}
