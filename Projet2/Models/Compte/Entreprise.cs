using System.Collections.Generic;
using Projet2.Models.Boutique;

namespace Projet2.Models.Compte
{
    public class Entreprise
    {
        public int Id { get; set; }
        public int NombreUtilisateur { get; set; }
        public int Siren { get; set; }
        public Adresse Adresse { get; set; }
        public int AdresseId { get; set; }


        public virtual List<Commande> CommandesBoutiqueEffectues { get; set; }


        public int AvisId { get; set; }
        public Avis Avis { get; set; }

    }
}
