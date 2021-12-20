using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Models.PanierSaisonniers
{
    interface IPanierSaisonnierService : IDisposable
    {
        void DeleteCreateDatabase();
        List<PanierSaisonnier> ObtientTousLesPaniers();
        int CreerPanierSaisonnier(List<Produit> produitsProposes, string description, string nomProducteur, decimal prix);
        int CreerPanierSaisonnier(PanierSaisonnier panierSaisonnier);
        int ModifierPanierSaisonnier(int Id, List<Produit> produitsProposes, string description, string nomProducteur, decimal prix);
        int ModifierPanierSaisonnier(PanierSaisonnier panierSaisonnier);
        void SupprimerPanierSaisonnier(int Id);
    }
}
