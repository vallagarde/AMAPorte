using Projet2.Models.Compte;
using System;
using System.Collections.Generic;

namespace Projet2.Models.Compte
{
    public interface ICompteServices : IDisposable
    {
        void DeleteCreateDatabase();
        List<Personne> ObtientToutesLesPersonnes();
        List<Adresse> ObtientToutesLesAdresses();
        List<AdA> ObtientTousLesAdAs();


        void CreerAdA(Personne personne, Identifiant identifiant, Adresse adresse);
        void ModifierAdA(AdA ada);
        void SupprimerAdA(int Id);

        int CreerAdresse(Adresse adresse);
        void ModifierAdresse(Adresse adresse);
        void SupprimerAdresse(int Id);

        int CreerPersonne(Personne personne);
        void ModifierPersonne(Personne personne);
        void SupprimerPersonne(int Id);


        // Authentification functions
        int AjouterIdentifiant(string userName, string password);
        void ModifierIdentifiant(Identifiant identifiant);
        void SupprimerIdentifiant(int Id);
        Identifiant Authentifier(string userName, string password);
        Identifiant ObtenirIdentifiant(int id);
        Identifiant ObtenirIdentifiant(string idStr);

    }
}
