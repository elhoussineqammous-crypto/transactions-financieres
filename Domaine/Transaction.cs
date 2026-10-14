using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace transactions_financieres.Domaine
{
    /// <summary>
    /// Classe de base commune à toutes les transactions (imposée par l'énoncé).
    /// </summary>
    /// <remarks>
    /// CHOIX : une classe abstraite plutôt qu'une interface, car les deux types partagent
    /// des données (date, montant, description) ET des contrôles (les invariants).
    /// Une interface obligerait à dupliquer ces contrôles dans chaque sous-classe (DRY).
    /// </remarks>
    public abstract class Transaction
    {
        // CHOIX : propriétés sans setter : la transaction est immuable.
        // Une opération financière enregistrée ne doit plus jamais changer.

        //Identifiant unique, utile pour un futur stockage en base.
        public Guid Identifiant { get; }

        //CHOIX : DateTimeOffset plutôt que DateTime, pour conserver le fuseau horaire
        //et éviter les ambiguïtés entre heure locale et heure UTC.
        public DateTimeOffset Date { get; }

        public decimal Montant { get; }

        public string Description { get; }

        //Type de la transaction, défini par chaque sous-classe
        public abstract TypeTransaction Type {  get; }

        // Effet de la transaction sur le solde.
        //CHOIX : polymorphisme. Chaque sous-classe sait ce qu'elle fait au solde,
        //donc le service n'a jamais besoin de tester le type de la transaction.
        public abstract decimal ImpactSurSolde();
        public override string ToString() => $"{Date:yyyy-MM-dd} {Type,-6} {Montant,12:N2} {Description}";

        protected Transaction(DateTimeOffset date, decimal montant, string description)
        {
           
            
            // RG-03 : la date est obligatoire
            if (date == default)
                throw new ArgumentException("La date de la transaction est obligatoire.", nameof(date));

            // RG-01 : le montant est strictement positif.
            if (montant <= 0)
                throw new ArgumentOutOfRangeException(nameof(montant), montant,
                "Le montant d'une transaction doit être strictement positif.");

            // RG-02 : la description est obligatoire
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La description de la transaction est obligatoire.",
                nameof(description));

            Identifiant = Guid.NewGuid();
            Date = date;
            Montant = montant;
            Description = description.Trim();
        }
    }
}
