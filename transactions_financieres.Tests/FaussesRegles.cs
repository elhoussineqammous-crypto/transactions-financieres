using System;
using System.Collections.Generic;
using System.Text;
using transactions_financieres.Domaine;
using transactions_financieres.Validation;

namespace transactions_financieres.Tests
{
    internal class FaussesRegles
    {
        // Fausses règles pour tester le service SEUL, sans dépendre des vraies règles.

        public sealed class RegleToujoursValide : IValidationRegle
        {
            public ValidationResult Validate(Transaction transaction, decimal soldeActuel) => ValidationResult.Succes();

        }

        public sealed class RegleToujoursEnEchec : IValidationRegle
        {
            private readonly string _message;

            public RegleToujoursEnEchec(string message) => _message = message;

            public ValidationResult Validate(Transaction transaction, decimal soldeActuel) => ValidationResult.Echec(_message);
        }
    }
}
