using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Data.Entities;

namespace UrunKatalogProjesi.Data.Models
{
    public class Category : Entity
    {
        public string CategoryName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
