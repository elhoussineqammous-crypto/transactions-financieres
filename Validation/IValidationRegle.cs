using System;
using System.Collections.Generic;
using System.Text;
using transactions_financieres.Domaine;


namespace transactions_financieres.Validation
{
    public interface  IValidationRegle
    {
        /// <summary>Contrat commun à toutes les règles de validation.</summary>
        /// <remarks>
        /// CHOIX : pattern Strategy. Chaque règle est une classe qui implémente cette interface
        /// Le service ne connaît que l'interface : pour ajouter une règle, on crée une nouvelle classe,
        /// sans modifier le code existant (exigence de l'énoncé, principe ouvert/fermé).
        /// </remarks>
        ValidationResult Validate(Transaction transaction, decimal soldeActuel );
    }
}
