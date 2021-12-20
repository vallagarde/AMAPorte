namespace Projet2.Models.Compte
{
    public class Admin
    {
        public int Id { get; set; }

        public Identifiant Identifiant { get; set; }
        public int IdentifiantId { get; set; }

        public bool EstGCCQ { get; set; }

        public bool EstGCRA { get; set; }

        public bool EstDSI { get; set; }

        public string Role
        {
            get
            {
                if (EstGCCQ) return "GCCQ";
                if (EstGCRA) return "GCRA";
                if (EstDSI) return "DSI";
                return null;
            }
            set
            {
                switch (value)
                {
                    case "GCCQ" :
                        EstGCCQ = true;
                        break;
                    case "GCRA":
                        EstGCRA = true;
                        break;
                    case "DSI":
                        EstDSI = true;
                        break;
                }
            }
        }

    }
}
