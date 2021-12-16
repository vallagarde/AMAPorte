﻿using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Projet2.Models.Compte
{
    public class CompteServices : ICompteServices
    {
        private readonly BddContext _bddContext;
        public CompteServices()
        {
            _bddContext = new BddContext();
        }

        public void DeleteCreateDatabase()
        {
            _bddContext.Database.EnsureDeleted();
            _bddContext.Database.EnsureCreated();
        }
        public void Dispose()
        {
            _bddContext.Dispose();
        }


        //Listes
        public List<Personne> ObtientToutesLesPersonnes()
        {
            return _bddContext.Personnes.ToList();
        }

        public List<Adresse> ObtientToutesLesAdresses()
        {
            return _bddContext.Adresses.ToList();
        }
        public List<AdA> ObtientTousLesAdAs()
        {
            return _bddContext.AdAs.ToList();
        }



        //Fonctions AdA
        public void CreerAdA(Personne personne, Identifiant identifiant, Adresse adresse)
        {
            CreerAdresse(adresse);
            
            personne.AdresseId = adresse.Id;
            CreerPersonne(personne);
            AdA adA = new AdA();
            adA.PersonneId = personne.Id;
            _bddContext.AdAs.Add(adA);
            _bddContext.SaveChanges();
        }

        public void ModifierAdA(AdA ada)
        {
            if (ada.Id != 0)
            {
                _bddContext.AdAs.Update(ada);
                _bddContext.SaveChanges();
            }
        }

        public void SupprimerAdA(int Id)
        {
            AdA adaASupprimer = _bddContext.AdAs.Find(Id);
            if (adaASupprimer != null)
            {
                Personne personneASupprimer = _bddContext.Personnes.Find(adaASupprimer.PersonneId);
                SupprimerAdresse(personneASupprimer.AdresseId);
                SupprimerIdentifiant(personneASupprimer.IdentifiantId);
                SupprimerPersonne(adaASupprimer.PersonneId);
                _bddContext.AdAs.Remove(adaASupprimer);
                _bddContext.SaveChanges();
            }
        }

        //Fonctions Adresse
        public int CreerAdresse(Adresse adresse)
        {
            _bddContext.Adresses.Add(adresse);
            _bddContext.SaveChanges();
            return adresse.Id;
        }

        public void ModifierAdresse(Adresse adresse)
        {
            if (adresse.Id != 0)
            {
                _bddContext.Adresses.Update(adresse);
                _bddContext.SaveChanges();
            }
        }

        public void SupprimerAdresse(int Id)
        {
            Adresse adresseASupprimer = _bddContext.Adresses.Find(Id);
            if (adresseASupprimer != null)
            {
                _bddContext.Adresses.Remove(adresseASupprimer);
                _bddContext.SaveChanges();
            }
        }

        //Fonctions Personne
        public int CreerPersonne(Personne personne)
        {
            _bddContext.Personnes.Add(personne);
            _bddContext.SaveChanges();
            return personne.Id;
        }

        public void ModifierPersonne(Personne personne)
        {
            if (personne.Id != 0)
            {
                _bddContext.Personnes.Update(personne);
                _bddContext.SaveChanges();
            }
        }

        public void SupprimerPersonne(int Id)
        {
            Personne personneASupprimer = _bddContext.Personnes.Find(Id);
            if (personneASupprimer != null)
            {
                _bddContext.Personnes.Remove(personneASupprimer);
                _bddContext.SaveChanges();
            }

        }

        //Fonctions Authentification
        public int AjouterIdentifiant(string adresseMail, string password)
        {
            string motDePasse = EncodeMD5(password);

            Identifiant identifiant = new Identifiant() { AdresseMail = adresseMail, MotDePasse = motDePasse };
            this._bddContext.Identifiants.Add(identifiant);
            this._bddContext.SaveChanges();
            return identifiant.Id;
        }

        public void ModifierIdentifiant(Identifiant identifiant)
        {
            if (identifiant.Id != 0)
            {
                string motDePasse = EncodeMD5(identifiant.MotDePasse);
                identifiant.MotDePasse = motDePasse;
                this._bddContext.Identifiants.Update(identifiant);
                this._bddContext.SaveChanges();
            }
        }

        public void SupprimerIdentifiant(int Id)
        {
            Identifiant identifiantASupprimer = _bddContext.Identifiants.Find(Id);
            if (identifiantASupprimer != null)
            { 
                this._bddContext.Identifiants.Remove(identifiantASupprimer);
                this._bddContext.SaveChanges();
            }
        }

        public Identifiant Authentifier(string adresseMail, string password)
        {
            string motDePasse = EncodeMD5(password);
            Identifiant identifiant = this._bddContext.Identifiants.FirstOrDefault(u => u.AdresseMail == adresseMail && u.MotDePasse == motDePasse);
            return identifiant;

        }
        public Identifiant ObtenirIdentifiant(int id)
        {
            return this._bddContext.Identifiants.Find(id);

        }
        public Identifiant ObtenirIdentifiant(string idStr)
        {
            if (int.TryParse(idStr, out int id))
            {
                return this.ObtenirIdentifiant(id);
            }
            return null;

        }

        public static string EncodeMD5(string motDePasse)
        {
            string motDePasseSel = "AMAPorte" + motDePasse + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(motDePasseSel)));
        }



    }
}
