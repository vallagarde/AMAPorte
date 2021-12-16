using System.Collections.Generic;

namespace Projet2.Models.Compte
{
    public class Paiement
        //A discuter pour les methodes
    {
        public int Id { get; set; }
        public string MethodePaiement { get; set; }

        
        public static List<Paiement> listePaiements = new List<Paiement>()
            {
                new Paiement() { Id = 1, MethodePaiement = "Visa"},
                new Paiement() { Id = 2, MethodePaiement = "PayPal" },
                new Paiement() { Id = 3, MethodePaiement = "Cheque" }
            };


        //public Paiement Visa = new Paiement() { Id = 1, MethodePaiement = "Visa"};
        //
        //public Paiement PayPal = new Paiement() { Id = 2, MethodePaiement = "PayPal" };
        //
        //public Paiement Cheque = new Paiement() { Id = 3, MethodePaiement = "Cheque" };


    }
}
