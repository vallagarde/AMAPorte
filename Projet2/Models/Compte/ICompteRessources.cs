using Projet2.Models.Compte;
using System;
using System.Collections.Generic;

namespace Projet2.Models.Compte
{
    public interface ICompteRessources : IDisposable
    {
        void DeleteCreateDatabase();
        List<Personne> ObtientToutesLesPersonnes();
        int CreerPersonne(string nom, string prenom, DateTime dateNaissance, string adresseMail, int nTelephone);
        int CreerPersonne(Personne personne);

        void ModifierPersonne(int id, string nom, string prenom, DateTime dateNaissance, string adresseMail, int nTelephone);

        void ModifierPersonne(Personne personne);

        void SupprimerPersonne(int Id);



        // Authentification functions
        int AjouterIdentifiant(string userName, string password);
        Identifiant Authentifier(string userName, string password);
        Identifiant ObtenirIdentifiant(int id);
        Identifiant ObtenirIdentifiant(string idStr);

    }
}
