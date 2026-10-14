using System;
using System.Collections.Generic;
using System.Text;

using transactions_financieres.Domaine;
using transactions_financieres.Validation;

namespace transactions_financieres.Regles
{
    /// <summary>RG-04 : le montant d'un crédit ne peut pas dépasser 10 000.</summary>
    public sealed class MontantMaximumCreditRG : IValidationRegle
    {
        private readonly decimal _plafond;

        //constructeur
        //le seuil est passé au constructeur (10000 par défaut) au lieu d'être écrit en dur
        public MontantMaximumCreditRG(decimal plafond = 10000m) => _plafond = plafond;
       
        ValidationResult IValidationRegle.Validate(Transaction transaction, decimal soldeActuel)
        {
            // RG-09 : cette règle ne concerne que les crédits ; un débit est toujours accepté ici
            if (transaction is not CreditTransaction)
                return ValidationResult.Succes();

            // CHOIX : "<=" car l'énoncé dit "ne peut pas dépasser" : 10000 est accepté
            if (transaction.Montant <= _plafond)
                return ValidationResult.Succes();

            return ValidationResult.Echec($"Le montant d'un crédit ne peut pas dépasser {_plafond:N2} (montant demandé :{transaction.Montant:N2}).");
        }
    }
}
