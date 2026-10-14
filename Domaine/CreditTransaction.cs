using System;
using System.Collections.Generic;
using System.Text;

namespace transactions_financieres.Domaine
{
    public class CreditTransaction : Transaction
    {
        public CreditTransaction(DateTimeOffset date, decimal montant, string description) : base(date, montant, description)
        {
        }

        public override TypeTransaction Type => TypeTransaction.CREDIT;

        public override decimal ImpactSurSolde() => Montant;
        
    }
}
