using System;

namespace EmailSender.Abstractions.DataAccess
{
    public interface ITransactionScope : IDisposable
    {
        public void Commit();

        public void Rollback();
    }
}
