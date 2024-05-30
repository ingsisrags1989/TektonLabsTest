using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.UnitOfWork
{
    public interface IUnitOfWorkAsync: IDisposable
    {
        Task<int> SaveChangesAsync();
        Task SyncObjectState<TEntity>(TEntity entity) where TEntity : class;
    }
}
