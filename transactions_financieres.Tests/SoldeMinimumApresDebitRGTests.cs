using System;
using System.Collections.Generic;
using System.Text;
using transactions_financieres.Regles;
using transactions_financieres.Validation;
using static transactions_financieres.Tests.DataMock;
namespace transactions_financieres.Tests
{
    // RG-05 : après un débit, le solde ne peut pas être inférieur à -5 000.
    public class SoldeMinimumApresDebitRGTests
    {
        private readonly IValidationRegle _regle = new SoldeMinimumApresDebitRG();

        [Theory]
        [InlineData("0", "5000", true)]       // 0 - 5000 = -5000 : pile la limite, accepté
        [InlineData("0", "5000.01", false)]   // -5000,01 : refusé
        public void Debit_Est_Accepte_Jusqu_A_Moins_5000(string solde, string debit, bool attenduValide)
        {
            var resultat = _regle.Validate(Debit(M(debit)), M(solde));

            Assert.Equal(attenduValide, resultat.EstValide);
        }

        [Fact]
        public void Un_Credit_N_Est_Pas_Concerne()
        {
            var resultat = _regle.Validate(Credit(10m), soldeActuel: -10000m);

            Assert.True(resultat.EstValide);
        }
    }
}
