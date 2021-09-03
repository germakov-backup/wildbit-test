using System.Data;
using EmailSender.Abstractions.DataAccess;

namespace EmailSender.Data
{
    /// <remarks>
    /// Turns out that postgres .net connector propagate transaction to DTC when using standard TransactionScope
    /// This class is a simple polyfill to abstract transaction to application layer
    /// </remarks>
    internal class SimpleTransactionScope : ITransactionScope
    {
        public SimpleTransactionScope(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        public IDbTransaction Transaction { get; }

        public bool IsDisposed { get; private set; }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!IsDisposed)
            {
                if (isDisposing)
                {
                    Transaction.Dispose();
                }

                IsDisposed = true;
            }
        }
    }
}
