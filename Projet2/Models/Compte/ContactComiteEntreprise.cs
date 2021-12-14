namespace Projet2.Models.Compte
{
    public class ContactComiteEntreprise
    {
        public int Id { get; set; }

        public Entreprise Entreprise { get; set; }

        public Personne Personne { get; set; }

        public AdA AdA { get; set; }

    }
}
