﻿namespace Projet2.Models.Compte
{
    public class Entreprise
    {
        public int Id { get; set; }
        public int NombreUtilisateur { get; set; }
        public int Siren { get; set; }
        ContactComiteEntreprise? ContactComiteEntreprise { get; set; }
    
    }
}
