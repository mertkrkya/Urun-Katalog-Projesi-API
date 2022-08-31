using System;

namespace UrunKatalogProjesi.Data.Entities
{
    public class Entity : CommonEntity
    {
        public int Id { get; set; }
    }

    public class CommonEntity
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
