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

        public AdA ObtenirAdAParIdentifiant(int id)
        {
            var query = from ada in _bddContext.AdAs where ada.Personne.IdentifiantId == id select ada;
            var adas = query.ToList();
            foreach (AdA ada in adas)
            {
                return ada;
            }
            return null;
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
        public AdA CreerAdA(Personne personne, Adresse adresse, AdA ada)
        {
            CreerAdresse(adresse);           
            personne.AdresseId = adresse.Id;
            CreerPersonne(personne);
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

        public void AjouterPhoto(string image, int identifiantId)
        {
            Identifiant identifiant = _bddContext.Identifiants.Find(identifiantId);
            if (identifiant.EstAdA)
            {
                AdA ada = ObtenirAdAParId(identifiantId);
                ada.Image = image; 
                _bddContext.AdAs.Update(ada);
                _bddContext.SaveChanges();
            }
            //else if (identifiant.EstAdP)
            //{
            //    AdP adp = ObtenirAdPParId(identifiantId);
            //    adp.Image = image;
            //    _bddContext.AdPs.Update(adp);
            //}
            //else if (identifiant.EstCE)
            //{
            //    ContactComiteEntreprise ce = ObtenirCCEParId(identifiantId);
            //    ce.Image = image;
            //    _bddContext.ContactComiteEntreprises.Update(ce);
            //}
        }

        //Obtenir AdP
        public List<AdP> ObtenirTousLesAdPs()
        {
            List<AdP> AdPList = _bddContext.AdPs.ToList();
            foreach (AdP adp in AdPList)
            {
                var queryPersonne = from personne in _bddContext.Personnes where personne.Id == adp.PersonneId select personne;
                adp.Personne = queryPersonne.First();
                var queryAdresse = from adresse in _bddContext.Adresses where adresse.Id == adp.Personne.AdresseId select adresse;
                var queryIdentifiant = from identifiant in _bddContext.Identifiants where identifiant.Id == adp.Personne.IdentifiantId select identifiant;
                adp.Personne.Adresse = queryAdresse.First();
                adp.Personne.Identifiant = queryIdentifiant.First();
            }
            return AdPList;
        }

        public List<AdP> ObtenirAdPsVedettes()
        {
            List<AdP> AdPList = _bddContext.AdPs.Where(c => (c.Vedette == true)).ToList();
            foreach (AdP adp in AdPList)
            {
                var queryPersonne = from personne in _bddContext.Personnes where personne.Id == adp.PersonneId select personne;
                adp.Personne = queryPersonne.First();
                var queryAdresse = from adresse in _bddContext.Adresses where adresse.Id == adp.Personne.AdresseId select adresse;
                var queryIdentifiant = from identifiant in _bddContext.Identifiants where identifiant.Id == adp.Personne.IdentifiantId select identifiant;
                adp.Personne.Adresse = queryAdresse.First();
                adp.Personne.Identifiant = queryIdentifiant.First();
            }
            return AdPList;
        }

        public AdP ObtenirAdPParIdentifiant(int id)
        {
            var query = from adp in _bddContext.AdPs where adp.Personne.IdentifiantId == id select adp;
            var adps = query.ToList();
            foreach (AdP adp in adps)
            {
                return adp;
            }
            return null;
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

        public AdP ObtenirAdPParId(int id)
        {
            AdP adp = (from a in _bddContext.AdPs where a.Id == id select a).FirstOrDefault();
            if (adp != null)
            {
                var queryPersonne = from personne in _bddContext.Personnes where personne.Id == adp.PersonneId select personne;
                adp.Personne = queryPersonne.First();
                var queryAdresse = from adresse in _bddContext.Adresses where adresse.Id == adp.Personne.AdresseId select adresse;
                var queryIdentifiant = from identifiant in _bddContext.Identifiants where identifiant.Id == adp.Personne.IdentifiantId select identifiant;
                adp.Personne.Adresse = queryAdresse.First();
                adp.Personne.Identifiant = queryIdentifiant.First();
                return adp;
            }
            return null;
        }


        public List<AdP> ObtenirAdPParNom(String nom)
        {
            List<AdP> list = ObtenirTousLesAdPs().FindAll(x => x.NomProducteur.ToLower().Contains(nom.ToLower())); ;
            return list;
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
                if (adpAModifier.EstEnAttente)
                {
                    adpAModifier.EstEnAttente = false;
                }
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

        public void ValidationAdP(AdP adp)
        {
            AdP adpAValider = (from a in _bddContext.AdPs where a.Id == adp.Id select a).FirstOrDefault();

            if (adp.EstActive)
            {
                adpAValider.EstActive = true;
                _bddContext.Update(adpAValider);
                _bddContext.SaveChanges();
            }
            else
            {
                adpAValider.AdminCommentaire = adp.AdminCommentaire;
                adpAValider.EstEnAttente = true;
                _bddContext.Update(adpAValider);
                _bddContext.SaveChanges();
            }

        }

            


//Obtenir CE
public List<ContactComiteEntreprise> ObtenirTousLesCCEs()
        {
            List<ContactComiteEntreprise> CCEList = _bddContext.ContactComiteEntreprises.ToList();
            foreach (ContactComiteEntreprise cce in CCEList)
            {
                var queryEntreprise = from entreprise in _bddContext.Entreprises where entreprise.Id == cce.EntrepriseId select entreprise;
                cce.Entreprise = queryEntreprise.First();
                var queryAdresse = from adresse in _bddContext.Adresses where adresse.Id == cce.Entreprise.AdresseId select adresse;
                var queryIdentifiant = from identifiant in _bddContext.Identifiants where identifiant.Id == cce.IdentifiantId select identifiant;
                cce.Entreprise.Adresse = queryAdresse.First();
                cce.Identifiant = queryIdentifiant.First();
            }
            return CCEList;

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


        public ContactComiteEntreprise ObtenirCCEParId(int id)
        {
            var query = from cce in _bddContext.ContactComiteEntreprises where cce.Id == id select cce;
            var cces = query.ToList();
            foreach (ContactComiteEntreprise cce in cces)
            {
                return cce;
            }
            return null;
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
        public int AjouterIdentifiant(Identifiant identifiant)
        {
            string motDePasse = EncodeMD5(identifiant.MotDePasse);

            identifiant.MotDePasse = motDePasse;
            this._bddContext.Identifiants.Add(identifiant);
            this._bddContext.SaveChanges();
            return identifiant.Id;
        }

        public bool TrouverIdentifiant(Identifiant identifiant)
        {
            bool adresseExistante = false;
            Identifiant identifiantExistant = (from i in _bddContext.Identifiants where i.AdresseMail == identifiant.AdresseMail select i).FirstOrDefault();
            if (identifiantExistant != null)
            {
                adresseExistante = true;
                return adresseExistante;
            }
            return adresseExistante;
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
            else
            {
                Identifiant identifiantExistant = (from i in _bddContext.Identifiants where i.AdresseMail == identifiant.AdresseMail select i).FirstOrDefault();
                string motDePasse = EncodeMD5(identifiant.MotDePasse);
                identifiant.MotDePasse = motDePasse;
                this._bddContext.Identifiants.Update(identifiant);
                this._bddContext.SaveChanges();
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

        public Client CreerClient(Client Client, Adresse adresse)
        {
            Client.Adresse = adresse;
            CreerAdresse(adresse);
            _bddContext.Clients.Add(Client);
            _bddContext.SaveChanges();
            return Client;
        }

    }
}
