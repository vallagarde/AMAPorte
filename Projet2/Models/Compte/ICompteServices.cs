using Projet2.Models.Compte;
using System;
using System.Collections.Generic;

namespace Projet2.Models.Compte
{
    public interface ICompteServices : IDisposable
    {
        void DeleteCreateDatabase();

        //Admin functions 
        List<Admin> ObtenirTousLesAdmins();
        Admin ObtenirAdminParIdentifiant(int id);
        Admin CreerAdmin(Admin admin);
        Admin ModifierAdmin(Admin admin);
        void SupprimerAdmin(int Id);

        //AdA functions
        List<AdA> ObtenirTousLesAdAs();
        AdA ObtenirAdAParPersonne(int id);
        AdA CreerAdA(Personne personne, Adresse adresse);
        AdA ModifierAdA(AdA ada);
        void SupprimerAdA(int Id);

        //AdP functions
        List<AdP> ObtenirTousLesAdPs();
        AdP ObtenirAdPParPersonne(int id);
        AdP CreerAdP(Personne personne, Adresse adresse, AdP adp);
        AdP ModifierAdP(AdP adpAModifier);
        void SupprimerAdP(int Id);

        //CE functions
        List<ContactComiteEntreprise> ObtenirTousLesCCEs();
        ContactComiteEntreprise ObtenirCCEParIdentifiant(int id);
        List<ContactComiteEntreprise> ObtenirCCEsParEntreprise(int id);
        ContactComiteEntreprise CreerCCE(ContactComiteEntreprise cce, Entreprise entreprise, Adresse adresse);
        ContactComiteEntreprise ModifierCCE(ContactComiteEntreprise cce);
        void SupprimerCCE(int Id);

        //Entreprise functions
        List<Entreprise> ObtenirTousLesEntreprises();
        Entreprise ObtenirEntreprise(int id);
        int CreerEntreprise(Entreprise entreprise, Adresse adresse);
        Entreprise ModifierEntreprise(Entreprise entreprise);
        void SupprimerEntreprise(int Id);

        //Adresse functions
        List<Adresse> ObtenirToutesLesAdresses();
        Adresse ObtenirAdresse(int id);
        int CreerAdresse(Adresse adresse);
        Adresse ModifierAdresse(Adresse adresse);
        void SupprimerAdresse(int Id);

        //Personne functions
        List<Personne> ObtenirToutesLesPersonnes();
        Personne ObtenirPersonne(int id);
        Personne ObtenirPersonneParIdentifiant(int id);
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
