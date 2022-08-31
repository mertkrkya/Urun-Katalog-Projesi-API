using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Models;

namespace UrunKatalogProjesi.Data.Repositories.Abstract
{
    public interface IConfigRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(string id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        Task InsertAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
