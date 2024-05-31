using Infrastructure.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.UnitOfWork
{
    public class UnitOfWorkAync : IUnitOfWorkAsync
    {
        private readonly ProductContext _context;
        private bool _disposed = false;

        public UnitOfWorkAync(ProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CurrentTransaction.CommitAsync();
        }

        public void RollbackTransaction()
        {
            _context.Database.CurrentTransaction.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Entry(entity).State = _context.Entry(entity).State;
        }
    }
}
