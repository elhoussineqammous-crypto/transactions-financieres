using System;
using System.Collections.Generic;
using System.Text;
using transactions_financieres.Domaine;
using transactions_financieres.Validation;

namespace transactions_financieres.Regles
{
    /// <summary>RG-05 : après un débit, le solde ne peut pas être inférieur à -5 000.</summary>
    public sealed class SoldeMinimumApresDebitRG : IValidationRegle
    {
        private readonly decimal _soldeMinimum;

        public SoldeMinimumApresDebitRG(decimal soldeMinimum = -5000m) => _soldeMinimum = soldeMinimum;
        
        ValidationResult IValidationRegle.Validate(Transaction transaction, decimal soldeActuel)
        {
            // RG - 09 : cette règle ne concerne que les débits; un crédit est toujours accepté ici.
            if (transaction is not DebitTransaction)
                return ValidationResult.Succes();

            // C'est un débit : on retire le montant du solde actuel.
            var soldeApresDebit = soldeActuel - transaction.Montant;

            // CHOIX : ">=" : un solde final de -5 000 pile est accepté.
            if (soldeApresDebit >= _soldeMinimum)
                return ValidationResult.Succes();

            return ValidationResult.Echec($"Le solde ne peut pas être inférieur à {_soldeMinimum:N2} après un débit (solde aprèsopération: { soldeApresDebit: N2}).");
        }
    }
}
