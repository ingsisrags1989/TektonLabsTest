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
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context)
        {
            _dbContext = context;
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
            var entityDb = await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entityDb.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var entityDb = _dbSet.Update(entity);
             await _dbContext.SaveChangesAsync();
            return entityDb.Entity;
        }
    }
}
