namespace Projet2.Models.Compte
{
    public class Admin
    {
        public int Id { get; set; }

        public Identifiant Identifiant { get; set; }
        public int IdentifiantId { get; set; }

        public static bool EstGCCQ { get; set; }

        public static bool EstGCRA { get; set; }

        public static bool EstDSI { get; set; }

    }
}
