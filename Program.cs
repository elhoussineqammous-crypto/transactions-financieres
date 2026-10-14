using System.Text;
using transactions_financieres.Domaine;
using transactions_financieres.Regles;
using transactions_financieres.Repositories;
using transactions_financieres.Services;
using transactions_financieres.Validation;

namespace transactions_financieres
{
    internal class Program
    {
        // Date de départ, puis on avance à chaque appel
        private static DateTimeOffset _dateCourante = new DateTimeOffset(2026, 10, 14, 9, 0, 0, TimeSpan.Zero);

        // Renvoie une date au hasard, toujours APRÈS la précédente
        private static DateTimeOffset DateSuivante()
        {
            int jours = Random.Shared.Next(1, 4);     // avance de 1 à 3 jours
            int heures = Random.Shared.Next(0, 9);    // et de 0 à 8 heures
            _dateCourante = _dateCourante.AddDays(jours).AddHours(heures);
            return _dateCourante;
        }
        static void Main(string[] args)
        {
            // Pour afficher correctement les accents dans la console
            Console.OutputEncoding = Encoding.UTF8;

           

            // 1. On choisit les règles à appliquer.
            //    AjouterTransaction une règle = ajouter une ligne ici. Le service ne change pas.
            var regles = new List<IValidationRegle>
            {
                new MontantMaximumCreditRG(),      // RG-04 : crédit <= 10 000
                new SoldeMinimumApresDebitRG()     // RG-05 : solde après débit >= -5 000
            };

            // 2. On crée le service avec son stockage et ses règles.
            var service = new TransactionService(new TransactionRepo(), regles);


            
           

            var ListeTransactions = new List<Transaction>
            {
                new CreditTransaction(DateSuivante(), 3000m, "Virement salaire"),
                new CreditTransaction(DateSuivante(), 15000m, "Dépôt 15,000.00 EUR"),
                new CreditTransaction(DateSuivante(), 10000m, "Dépôt 10,000.00 EUR"),
                new DebitTransaction(DateSuivante(),  12000m, "Achat dans Amazon"),
                new DebitTransaction(DateSuivante(),   5000m, "Paiement fournisseur (solde à -5,000.00 EUR)"),
                new DebitTransaction(DateSuivante(),   0.01m, "Prélèvement de trop"),
                new CreditTransaction(DateSuivante(),  1500m, "Remboursement client")
            };


            // 3. On ajoute chaque transaction et on affiche le résultat.
            foreach (var transaction in ListeTransactions)
            {
                var resultat = service.AjouterTransaction(transaction);
                var statut = resultat.EstValide ? "ACCEPTÉE" : "REFUSÉE ";

                Console.WriteLine($"[{statut}] {transaction}");

                foreach (var erreur in resultat.Erreurs)
                    Console.WriteLine($"           -> {erreur}");

                Console.WriteLine($"           Solde : {service.ObtenirSolde():N2}");
            }

            // 5. Le bilan final.
            Console.WriteLine();
            Console.WriteLine($"Transactions enregistrées : {service.ListerTransactions().Count}");
            Console.WriteLine($"Solde final : {service.ObtenirSolde():N2}");

          
    }
    }
}
