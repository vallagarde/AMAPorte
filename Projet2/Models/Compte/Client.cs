using Projet2.Models.Compte;
using System.Collections.Generic;

namespace Projet2.Models.Compte

{
    public class Client
    {
        public int Id { get; set; }
        public int adresseIP { get; set; }
        public virtual List<Personne> Personnes { get; set; }

    }
}