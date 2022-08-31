using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Context;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Repositories.Abstract;

namespace UrunKatalogProjesi.Data.Repositories.Concrete
{
    public class ConfigRepository<TEntity> : IConfigRepository<TEntity> where TEntity : class 
    {
        protected readonly ConfigDbContext _configDbContext;
        private readonly DbSet<TEntity> _dbSet;
        public ConfigRepository(ConfigDbContext configDbContext)
        {
            _configDbContext = configDbContext;
            _dbSet = _configDbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            var result = await _dbSet.FindAsync(id);
            if (result != null)
                _configDbContext.Entry(result).State = EntityState.Detached;
            return result;
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
