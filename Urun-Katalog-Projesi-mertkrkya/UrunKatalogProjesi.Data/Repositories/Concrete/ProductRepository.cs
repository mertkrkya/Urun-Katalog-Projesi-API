using UrunKatalogProjesi.Data.Context;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Repositories.Abstract;

namespace UrunKatalogProjesi.Data.Repositories.Concrete
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        protected readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
