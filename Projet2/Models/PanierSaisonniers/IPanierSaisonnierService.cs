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
        int CreerPanierSaisonnier(string nomPanier, string nomProducteur, string produitsProposes, string description, decimal prix, string image);
        int CreerPanierSaisonnier(PanierSaisonnier panierSaisonnier);
        int ModifierPanierSaisonnier(int Id, string nomPanier, string nomProducteur, string produitsProposes, string description, decimal prix);
        int ModifierPanierSaisonnier(PanierSaisonnier panierSaisonnier);
        void SupprimerPanierSaisonnier(int Id);
    }
}
