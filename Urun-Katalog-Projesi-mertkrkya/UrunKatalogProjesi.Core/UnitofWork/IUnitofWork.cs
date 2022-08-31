using System.Threading.Tasks;

namespace UrunKatalogProjesi.Data.UnitofWork
{
    public interface IUnitofWork
    {
        Task CommitAsync();
        void Commit();
    }
}