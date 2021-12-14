namespace Projet2.Models
{
    public class Entreprise
    {
        public int Id { get; set; }
        public int NombreUtilisateur { get; set; }
        public int Siren { get; set; }
        ContactComiteEntreprise? ContactComiteEntreprise { get; set; }
    
    }
}
