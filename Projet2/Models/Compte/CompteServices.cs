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


        //Obtenir Admin
        public List<Admin> ObtenirTousLesAdmins()
        {
            return _bddContext.Admins.ToList();
        }
        public Admin ObtenirAdminParIdentifiant(int id)
        {
            var query = from admin in _bddContext.Admins where admin.IdentifiantId == id select admin;
            var admins = query.ToList();
            foreach (Admin admin in admins)
            {
                return admin;
            }
            return null;

        }

        // Fonctions Admin
        public Admin CreerAdmin(Admin admin)
        {
            _bddContext.Admins.Add(admin);
            _bddContext.SaveChanges();
            return admin;
        }
        public Admin ModifierAdmin(Admin admin)
        {
            if (admin.Id != 0)
            {
                _bddContext.Admins.Update(admin);
                _bddContext.SaveChanges();
                return admin;
            }
            return null;

        }
        public void SupprimerAdmin(int Id)
        {
            Admin adminASupprimer = _bddContext.Admins.Find(Id);
            if (adminASupprimer != null)
            {
                _bddContext.Admins.Remove(adminASupprimer);
                _bddContext.SaveChanges();
            }
        }

        //Obtenir AdA
        public List<AdA> ObtenirTousLesAdAs()
        {
            List<AdA> AdAList = _bddContext.AdAs.ToList();
            foreach (AdA ada in AdAList)
            {
                var queryPersonne = from personne in _bddContext.Personnes where personne.Id == ada.PersonneId select personne;                
                ada.Personne = queryPersonne.First();
                var queryAdresse = from adresse in _bddContext.Adresses where adresse.Id == ada.Personne.AdresseId select adresse;
                var queryIdentifiant = from identifiant in _bddContext.Identifiants where identifiant.Id == ada.Personne.IdentifiantId select identifiant;
                ada.Personne.Adresse = queryAdresse.First();
                ada.Personne.Identifiant = queryIdentifiant.First();

            }
            return AdAList;
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

        public AdA ObtenirAdAParId(int id)
        {
            var query = from ada in _bddContext.AdAs where ada.Id == id select ada;
            var adas = query.ToList();
            foreach (AdA ada in adas)
            {
                return ada;
            }
            return null;

        }

        //Fonctions AdA
        public AdA CreerAdA(Personne personne, Adresse adresse)
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
        public List<AdP> ObtenirTousLesAdPs()
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
        public AdP CreerAdP(Personne personne, Adresse adresse, AdP adp)
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


        //Obtenir CE
        public List<ContactComiteEntreprise> ObtenirTousLesCCEs()
        {
            return _bddContext.ContactComiteEntreprises.ToList();

        }
        public ContactComiteEntreprise ObtenirCCEParIdentifiant(int id)
        {
            var query = from cce in _bddContext.ContactComiteEntreprises where cce.IdentifiantId == id select cce;
            var cces = query.ToList();
            foreach (ContactComiteEntreprise cce in cces)
            {
                return cce;
            }
            return null;

        }
        public List<ContactComiteEntreprise> ObtenirCCEsParEntreprise(int id)
        {
            var query = from cce in _bddContext.ContactComiteEntreprises where cce.EntrepriseId == id select cce;
            var cces = query.ToList();
            return cces;

        }

        //Fonctions CE
        public ContactComiteEntreprise CreerCCE(ContactComiteEntreprise cce, Entreprise entreprise, Adresse adresse)
        {
            cce.EntrepriseId = CreerEntreprise(entreprise, adresse);
            _bddContext.ContactComiteEntreprises.Add(cce);
            _bddContext.SaveChanges();
            return cce;
        }
        public ContactComiteEntreprise ModifierCCE(ContactComiteEntreprise cceAModifier)
        {
            if (cceAModifier.Id != 0)
            {
                _bddContext.ContactComiteEntreprises.Update(cceAModifier);
                _bddContext.SaveChanges();
                return cceAModifier;
            }
            return null;
        }
        public void SupprimerCCE(int Id)
        {
            ContactComiteEntreprise cceASupprimer = _bddContext.ContactComiteEntreprises.Find(Id);
            _bddContext.ContactComiteEntreprises.Remove(cceASupprimer);
            _bddContext.SaveChanges();
            SupprimerIdentifiant(cceASupprimer.IdentifiantId);

        }

        //Obtenir Entreprise
        public List<Entreprise> ObtenirTousLesEntreprises()
        {
            return _bddContext.Entreprises.ToList();
        }
        public Entreprise ObtenirEntreprise(int id)
        {
            Entreprise entreprise = _bddContext.Entreprises.Find(id);
            return entreprise;
        }

        //Fonctions Entrprise
        public int CreerEntreprise(Entreprise entreprise, Adresse adresse)
        {
            
            var queryEntrepriseExistant = from e in _bddContext.Entreprises where e.Siren == entreprise.Siren select e;
            var queryAdresseExistant = from a in _bddContext.Adresses where a == adresse select a;

            if (queryEntrepriseExistant.Any())
            {
                var entrepriseExistantList = queryEntrepriseExistant.ToList();
                Entreprise entrepriseExistant = entrepriseExistantList.First();
                return entrepriseExistant.Id;
            }
            else
            {
                if (queryAdresseExistant.Any())
                {
                    var adresseExistantList = queryAdresseExistant.ToList();
                    Adresse adresseExistante = adresseExistantList.First();
                    entreprise.AdresseId = adresseExistante.Id;
                    _bddContext.Entreprises.Add(entreprise);
                    _bddContext.SaveChanges();
                    return entreprise.Id;
                }
                else
                {
                    entreprise.AdresseId = CreerAdresse(adresse);
                    _bddContext.Entreprises.Add(entreprise);
                    _bddContext.SaveChanges();
                    return entreprise.Id;
                }
            }
        }
        public Entreprise ModifierEntreprise(Entreprise entrepriseAModifier)
        {
            if (entrepriseAModifier.Id != 0)
            {
                _bddContext.Entreprises.Update(entrepriseAModifier);
                this._bddContext.SaveChanges();
                return entrepriseAModifier;
            }
            return null;
        }
        public void SupprimerEntreprise(int Id)
        {
            Entreprise entrepriseASupprimer = _bddContext.Entreprises.Find(Id);
            List<ContactComiteEntreprise> contactComiteEntreprises = ObtenirCCEsParEntreprise(Id);
            if (entrepriseASupprimer != null)
            {
                _bddContext.Entreprises.Remove(entrepriseASupprimer);

                foreach (ContactComiteEntreprise contactComiteEntreprise in contactComiteEntreprises)
                {
                    if (contactComiteEntreprises.Count != 0)
                    {
                        SupprimerCCE(contactComiteEntreprise.Id);
                        SupprimerIdentifiant(contactComiteEntreprise.IdentifiantId);
                    }
                }
                SupprimerAdresse(entrepriseASupprimer.AdresseId);
                this._bddContext.SaveChanges();
            }
        }

        //Obtenir Adresse
        public List<Adresse> ObtenirToutesLesAdresses()
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
                this._bddContext.SaveChanges();
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
                this._bddContext.SaveChanges();
            }
        }


        //Obtenir Personne
        public List<Personne> ObtenirToutesLesPersonnes()
        {
            return _bddContext.Personnes.ToList();
        }
        public Personne ObtenirPersonne(int id)
        {
            Personne personne = _bddContext.Personnes.Find(id);
            if (personne != null)
            {
                return personne;
            }
            return null;
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
                this._bddContext.SaveChanges();
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
