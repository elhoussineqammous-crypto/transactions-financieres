using System;
using System.Collections.Generic;
using System.Text;
using transactions_financieres.Domaine;
using transactions_financieres.Regles;
using transactions_financieres.Validation;
using static transactions_financieres.Tests.DataMock;

namespace transactions_financieres.Tests
{
    // RG-04 : un crédit ne peut pas dépasser 10 000.
    public class MontantMaximumCreditRGTests
    {
        // Typée avec l'interface, car Validate est une implémentation explicite.
        private readonly IValidationRegle _regle = new MontantMaximumCreditRG();

        [Theory]
        [InlineData("10000", true)]       // pile le plafond : accepté
        [InlineData("10000.01", false)]   // juste au-dessus : refusé
        public void Credit_Est_Accepte_Jusqu_Au_Plafond(string montant, bool attenduValide)
        {
            var resultat = _regle.Validate(Credit(M(montant)), soldeActuel: 0m);

            Assert.Equal(attenduValide, resultat.EstValide);
        }


        [Fact]
        public void Un_Debit_N_Est_Pas_Concerne()
        {
            var resultat = _regle.Validate(Debit(50000m), soldeActuel: 100000m);

            Assert.True(resultat.EstValide);
        }
    }
}
