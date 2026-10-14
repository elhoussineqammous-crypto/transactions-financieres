using System;
using System.Collections.Generic;
using System.Text;
using transactions_financieres.Domaine;
using transactions_financieres.Repositories;
using transactions_financieres.Validation;

namespace transactions_financieres.Services
{
    public sealed class TransactionService
    {
        private readonly ITransactionRepo _depot;
        private readonly List<IValidationRegle> _regles;

        public TransactionService(ITransactionRepo depot, IEnumerable<IValidationRegle> regles)
        {
            ArgumentNullException.ThrowIfNull(depot);
            ArgumentNullException.ThrowIfNull(regles);

            _depot = depot;
            _regles = regles.ToList();

        }

        //RG-06 : solde initial à 0, puis somme des effets de toutes les transactions enregistrées.
        public decimal ObtenirSolde() => _depot.listerToutesTransactions().Sum(t => t.ImpactSurSolde());

        //Vérifie une transaction avec toutes les règles, sans l'enregistrer.
        public ValidationResult Valider(Transaction transaction)
        {
            ArgumentNullException.ThrowIfNull(transaction);

            var soldeActuel = ObtenirSolde();

            // RG-08 : on interroge TOUTES les règles et on rassemble toutes leurs erreurs.
            var erreurs = _regles.SelectMany(regle => regle.Validate(transaction, soldeActuel).Erreurs);

            return new ValidationResult(erreurs);
        }

        //RG-07 : enregistre la transaction seulement si elle est valide.
        public ValidationResult AjouterTransaction(Transaction transaction)
        {
            var resultat = Valider(transaction);

            if (resultat.EstValide)
                _depot.AjouterTransaction(transaction);

            return resultat;
        }

        public IReadOnlyList<Transaction> ListerTransactions() => _depot.listerToutesTransactions();
    }
}
