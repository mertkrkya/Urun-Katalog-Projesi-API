using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UrunKatalogProjesi.Data.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id); 
        Task<IEnumerable<TEntity>> GetAllAsync();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        Task InsertAsync(TEntity entity);
        void Update(TEntity entity); //Uzun işlem olmadığı için senkron da yapılabilir. Direkt stateyi değişiyor.
        void Delete(TEntity entity);
    }
}
