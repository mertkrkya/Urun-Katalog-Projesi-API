using UrunKatalogProjesi.Data.Context;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Repositories.Abstract;

namespace UrunKatalogProjesi.Data.Repositories.Concrete
{
    public class OfferRepository : BaseRepository<Offer>, IOfferRepository
    {
        protected readonly AppDbContext _appDbContext;
        public OfferRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
