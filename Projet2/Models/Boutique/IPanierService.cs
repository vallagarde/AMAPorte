using System;
using System.Collections.Generic;

namespace Projet2.Models.Boutique
{
    public interface IPanierService : IDisposable
    {
        int CreerLigneBoutique(int quantite, Article article, decimal sousTotal);
        int ModifierLigne(int id ,int quantite, Article article, decimal sousTotal);
        int CreerPanier(List<LignePanierBoutique> liste);
        int ModifierPanier(int id ,List<LignePanierBoutique> liste);
    }
}
