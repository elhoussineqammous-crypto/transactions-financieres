using System;
using System.Collections.Generic;
using System.Text;
using transactions_financieres.Domaine;

namespace transactions_financieres.Repositories
{
    public  interface ITransactionRepo
    {
        void AjouterTransaction(Transaction  transaction);

        IReadOnlyList<Transaction> listerToutesTransactions();
    }
}
