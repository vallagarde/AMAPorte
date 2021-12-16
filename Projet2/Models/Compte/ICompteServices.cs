﻿using Projet2.Models.Compte;
using System;
using System.Collections.Generic;

namespace Projet2.Models.Compte
{
    public interface ICompteServices : IDisposable
    {
        void DeleteCreateDatabase();

        //AdA functions
        List<AdA> ObtientTousLesAdAs();
        AdA CreerAdA(Personne personne, Identifiant identifiant, Adresse adresse);
        AdA ModifierAdA(AdA ada);
        void SupprimerAdA(int Id);

        //Adresse functions
        List<Adresse> ObtientToutesLesAdresses();
        Adresse ObtenirAdresse(int id);
        int CreerAdresse(Adresse adresse);
        Adresse ModifierAdresse(Adresse adresse);
        void SupprimerAdresse(int Id);

        //Personne functions
        List<Personne> ObtientToutesLesPersonnes();
        Personne ObtenirPersonne(int id);
        int CreerPersonne(Personne personne);
        Personne ModifierPersonne(Personne personne);
        void SupprimerPersonne(int Id);


        // Authentification functions
        int AjouterIdentifiant(string userName, string password);
        Identifiant ModifierIdentifiant(Identifiant identifiant);
        void SupprimerIdentifiant(int Id);
        Identifiant Authentifier(string userName, string password);
        Identifiant ObtenirIdentifiant(int id);
        Identifiant ObtenirIdentifiant(string idStr);

    }
}