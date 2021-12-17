using System.Text;
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


        //Obtenir AdA
        public List<AdA> ObtientTousLesAdAs()
        {
            return _bddContext.AdAs.ToList();
        }
        public AdA ObtenirAdAParPersonne(int id)
        {
            var query = from ada in _bddContext.AdAs where ada.PersonneId == id select ada;
            var adas = query.ToList();
            foreach (AdA ada in adas)
            {
                return ada;
            }
            return null;
        }

        //Fonctions AdA
        public AdA CreerAdA(Personne personne, Identifiant identifiant, Adresse adresse)
        {
            CreerAdresse(adresse);           
            personne.AdresseId = adresse.Id;
            CreerPersonne(personne);
            AdA ada = new AdA();
            ada.PersonneId = personne.Id;
            _bddContext.AdAs.Add(ada);
            _bddContext.SaveChanges();
            return ada;
        }

        public AdA ModifierAdA(AdA adaAModifier)
        {   
            if (adaAModifier.Id != 0)
            {
                _bddContext.AdAs.Update(adaAModifier);
                _bddContext.SaveChanges();
                return adaAModifier;
            }
            return null;
        }

        public void SupprimerAdA(int Id)
        {
            AdA adaASupprimer = _bddContext.AdAs.Find(Id);
            if (adaASupprimer != null)
            {
                _bddContext.AdAs.Remove(adaASupprimer);
                Personne personneASupprimer = _bddContext.Personnes.Find(adaASupprimer.PersonneId);
                SupprimerAdresse(personneASupprimer.AdresseId);
                SupprimerIdentifiant(personneASupprimer.IdentifiantId);
                SupprimerPersonne(personneASupprimer.Id);          
                _bddContext.SaveChanges();
            }
        }

        //Obtenir AdP
        public List<AdP> ObtientTousLesAdPs()
        {
            return _bddContext.AdPs.ToList();
        }

        public AdP ObtenirAdPParPersonne(int id)
        {
            var query = from adp in _bddContext.AdPs where adp.PersonneId == id select adp;
            var adps = query.ToList();
            foreach (AdP adp in adps)
            {
                return adp;
            }
            return null;
        }

        //Fonctions AdP
        public AdP CreerAdP(Personne personne, Identifiant identifiant, Adresse adresse, AdP adp)
        {
            personne.Adresse = adresse;
            CreerAdresse(adresse);
            CreerPersonne(personne);
            adp.PersonneId = personne.Id;
            _bddContext.AdPs.Add(adp);
            _bddContext.SaveChanges();
            return adp;
        }

        public AdP ModifierAdP(AdP adpAModifier)
        {
            if (adpAModifier.Id != 0)
            {
                _bddContext.AdPs.Update(adpAModifier);
                _bddContext.SaveChanges();
                return adpAModifier;
            }
            return null;
        }

        public void SupprimerAdP(int Id)
        {
            AdP adpASupprimer = _bddContext.AdPs.Find(Id);
            if (adpASupprimer != null)
            {
                _bddContext.AdPs.Remove(adpASupprimer);
                Personne personneASupprimer = _bddContext.Personnes.Find(adpASupprimer.PersonneId);
                SupprimerAdresse(personneASupprimer.AdresseId);
                SupprimerIdentifiant(personneASupprimer.IdentifiantId);
                SupprimerPersonne(personneASupprimer.Id);
                _bddContext.SaveChanges();
            }
        }

        //Obtenir Adresse
        public List<Adresse> ObtientToutesLesAdresses()
        {
            return _bddContext.Adresses.ToList();
        }

        public Adresse ObtenirAdresse(int id)
        {
            Adresse adresse = _bddContext.Adresses.Find(id);
            return adresse;
        }

        //Fonctions Adresse
        public int CreerAdresse(Adresse adresse)
        {
            _bddContext.Adresses.Add(adresse);
            _bddContext.SaveChanges();
            return adresse.Id;
        }

        public Adresse ModifierAdresse(Adresse adresse)
        {
            if (adresse.Id != 0)
            {
                _bddContext.Adresses.Update(adresse);
                _bddContext.SaveChanges();
                return adresse;
            }
            return null;
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

        

        //Obtenir Personne
        public List<Personne> ObtientToutesLesPersonnes()
        {
            return _bddContext.Personnes.ToList();
        }
        public Personne ObtenirPersonne(int id)
        {
            Personne personne = _bddContext.Personnes.Find(id);
            return personne;
        }
        public Personne ObtenirPersonneParIdentifiant(int id)
        {
            var query = from personne in _bddContext.Personnes where personne.IdentifiantId == id select personne;
            var personnes = query.ToList();
            foreach (Personne personne in personnes)
            {
                return personne;
            }
            return null;
        }

        //Fonctions Personne
        public int CreerPersonne(Personne personne)
        {
            _bddContext.Personnes.Add(personne);
            _bddContext.SaveChanges();
            return personne.Id;
        }

        public Personne ModifierPersonne(Personne personne)
        {
            if (personne.Id != 0)
            {
                _bddContext.Personnes.Update(personne);
                _bddContext.SaveChanges();
                return personne;
            }
            return null;
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

        public Identifiant ModifierIdentifiant(Identifiant identifiant)
        {
            if (identifiant.Id != 0)
            {
                string motDePasse = EncodeMD5(identifiant.MotDePasse);
                identifiant.MotDePasse = motDePasse;
                this._bddContext.Identifiants.Update(identifiant);
                this._bddContext.SaveChanges();
                return identifiant;
            }
            return identifiant;
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
