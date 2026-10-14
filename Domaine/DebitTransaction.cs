using System;
using System.Collections.Generic;
using System.Text;

namespace transactions_financieres.Domaine
{
    public class DebitTransaction : Transaction
    {
        public DebitTransaction(DateTimeOffset date, decimal montant, string description) : base(date, montant, description)
        {
        }

        public override TypeTransaction Type => TypeTransaction.DEBIT;

        public override decimal ImpactSurSolde() => -Montant;
       
    }
}
