using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using Projet2.Models.Compte;
using System;
using System.Linq;

namespace Projet2.Models
{
    public class Dal : IDal
    {
        private readonly BddContext _bddContext;
        public Dal()
        {
            _bddContext = new BddContext();
        }

        public void DeleteCreateDatabase()
        {
            _bddContext.Database.EnsureDeleted();
            _bddContext.Database.EnsureCreated();
        }

        public List<Personne> ObtientToutesLesPersonnes()
        {
            return _bddContext.Personnes.ToList();
        }


        public void Dispose()
        {
            _bddContext.Dispose();
        }

        public int CreerPersonne(string nom, string prenom, DateTime dateNaissance, string adresseMail, int nTelephone)
        {
            Personne personne = new Personne() { Nom = nom, Prenom = prenom, DateNaissance = dateNaissance, AdresseMail = adresseMail, NTelephone = nTelephone };
            _bddContext.Personnes.Add(personne);
            _bddContext.SaveChanges();
            return personne.Id;
        }

        public int CreerPersonne(Personne personne)
        {
            _bddContext.Personnes.Add(personne);
            _bddContext.SaveChanges();
            return personne.Id;
        }

        public void ModifierPersonne(int id, string nom, string prenom, DateTime dateNaissance, string adresseMail, int nTelephone)
        {
            Personne personne = _bddContext.Personnes.Find(id);

            if (personne != null)
            {
                personne.Nom = nom;
                personne.Prenom = prenom;
                personne.DateNaissance = dateNaissance;
                personne.AdresseMail = adresseMail;
                personne.NTelephone = nTelephone;
                _bddContext.SaveChanges();
            }

        }

        public void ModifierPersonne(Personne personne)
        {
            //if (personne.Id != null)
            //{
                _bddContext.Personnes.Update(personne);
                _bddContext.SaveChanges();
            //}
        }


        public int AjouterPersonne(string prenom, string password)
        {
            string motDePasse = EncodeMD5(password);
            Personne personne = new Personne() { Prenom = prenom, MotDePasse = motDePasse };
            this._bddContext.Personnes.Add(personne);
            this._bddContext.SaveChanges();
            return personne.Id;
        }
        public Personne Authentifier(string prenom, string password)
        {
            string motDePasse = EncodeMD5(password);
            Personne personne = this._bddContext.Personnes.FirstOrDefault(u => u.Prenom == prenom && u.MotDePasse == motDePasse);
            return personne;

        }
        public Personne ObtenirPersonne(int id)
        {
            return this._bddContext.Personnes.Find(id);

        }
        public Personne ObtenirPersonne(string idStr)
        {
            if (int.TryParse(idStr, out int id))
            {
                return this.ObtenirPersonne(id);
            }
            return null;

        }

        public static string EncodeMD5(string motDePasse)
        {
            string motDePasseSel = "ChoixResto" + motDePasse + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(motDePasseSel)));
        }



    }
}
