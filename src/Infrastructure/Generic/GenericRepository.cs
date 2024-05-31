using Infrastructure.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context, IUnitOfWorkAsync unitOfWork)
        {
            _dbContext = context;
            _unitOfWorkAsync = unitOfWork;
            if (_dbContext != null)
            {
                _dbSet = _dbContext.Set<TEntity>();
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = _dbSet;
            return query;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
            var entityDb = await _dbSet.AddAsync(entity);
            _unitOfWorkAsync.SyncObjectState(entity);
            return entityDb.Entity;
        }

        public TEntity UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _unitOfWorkAsync.SyncObjectState(entity);
            return  entity;
        }
    }
}
