using System;
using System.Collections.Generic;
using System.Text;
using transactions_financieres.Domaine;

namespace transactions_financieres.Repositories
{
    public sealed class TransactionRepo : ITransactionRepo
    {
        private readonly List<Transaction> _transactions = new List<Transaction>();
        void ITransactionRepo.AjouterTransaction(Transaction transaction)
        {
            ArgumentNullException.ThrowIfNull(transaction);
            _transactions.Add(transaction);
        }

        IReadOnlyList<Transaction> ITransactionRepo.listerToutesTransactions()
        {
            return _transactions.AsReadOnly();
        }
    }
}
