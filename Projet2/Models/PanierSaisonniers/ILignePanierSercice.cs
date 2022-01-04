using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2.Models.PanierSaisonniers
{
    interface ILignePanierSercice : IDisposable
    {
        LignePanierSaisonnier CreerLignePanier(int quantite, int PanierSaisonnierId, decimal sousTotal, int semaine);
        int ModifierLignePanier(int id, int quantite, PanierSaisonnier panierSaisonnier, decimal sousTotal);
    }
}
