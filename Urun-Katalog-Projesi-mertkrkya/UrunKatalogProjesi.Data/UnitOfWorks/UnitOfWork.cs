using System.Threading.Tasks;
using UrunKatalogProjesi.Data.UnitofWork;
using UrunKatalogProjesi.Data.Context;

namespace JUrunKatalogProjesi.Core.UnitOfWorks
{
    public class UnitOfWork : IUnitofWork
    {
        private readonly AppDbContext dbContext;
        public UnitOfWork(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
