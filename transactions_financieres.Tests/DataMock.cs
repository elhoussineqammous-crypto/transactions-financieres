using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using transactions_financieres.Domaine;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace transactions_financieres.Tests
{
    internal class DataMock
    {
        // Date fixe : les tests donnent toujours le même résultat.
        public static readonly DateTimeOffset Jour = new(2026, 10, 12, 10, 0, 0, TimeSpan.Zero);

        // [InlineData] n'accepte pas les decimal : on écrit les montants en texte ("10000.01").
        public static decimal M(string montant) => decimal.Parse(montant, CultureInfo.InvariantCulture);

        public static CreditTransaction Credit(decimal montant) => new CreditTransaction(Jour, montant, "Crédit de test");

        public static DebitTransaction Debit(decimal montant) => new DebitTransaction(Jour, montant, "Débit de test");
    }
}
