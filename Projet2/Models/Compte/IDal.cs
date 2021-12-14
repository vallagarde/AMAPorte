using Projet2.Models.Compte;
using System;
using System.Collections.Generic;

namespace Projet2.Models.Compte
{
    public interface IDal : IDisposable
    {
        void DeleteCreateDatabase();
        List<Personne> ObtientToutesLesPersonnes();
        int CreerPersonne(string nom, string prenom, DateTime dateNaissance, string adresseMail, int nTelephone);
        int CreerPersonne(Personne personne);

        void ModifierPersonne(int id, string nom, string prenom, DateTime dateNaissance, string adresseMail, int nTelephone);

        void ModifierPersonne(Personne personne);

        //SUPPRIMER

        // Authentification functions

        int AjouterPersonne(string nom, string password);
        Personne  Authentifier(string nom, string password);
        Personne ObtenirPersonne(int id);
        Personne ObtenirPersonne(string idStr);

    }
}
