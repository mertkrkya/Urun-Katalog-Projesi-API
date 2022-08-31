using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Context;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Repositories.Abstract;

namespace UrunKatalogProjesi.Data.Repositories.Concrete
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        protected readonly AppDbContext _appDbContext;
        private readonly DbSet<Category> _dbSet;

        public CategoryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.Set<Category>(); //Context'ten set edilir.

        }

        public async override Task<Category> GetByIdAsync(int id)
        {
            var result = await _dbSet.Include(r => r.Products).AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            return result;
        }
    }
}
