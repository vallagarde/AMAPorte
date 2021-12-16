using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Projet2.Models.Compte
{
    public class CompteRessources : ICompteRessources
    {
        private readonly BddContext _bddContext;
        public CompteRessources()
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

        public int AjouterIdentifiant(string userName, string password)
        {
            string motDePasse = EncodeMD5(password);
            Identifiant identifiant = new Identifiant() { UserName = userName, MotDePasse = motDePasse };
            this._bddContext.Identifiants.Add(identifiant);
            this._bddContext.SaveChanges();
            return identifiant.Id;
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

        public Identifiant Authentifier(string userName, string password)
        {
            string motDePasse = EncodeMD5(password);
            Identifiant identifiant = this._bddContext.Identifiants.FirstOrDefault(u => u.UserName == userName && u.MotDePasse == motDePasse);
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
