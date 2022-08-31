using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Entities;

namespace UrunKatalogProjesi.Data.Models
{
    public class Product : Entity
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string ImageLink { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        /// <summary>
        /// ProductStatus durumu ürün kondisyonu olarak düşünülmüştür.
        /// </summary>
        public ProductStatuses ProductStatus { get; set; }
        public decimal Price { get; set; }
        public bool isOfferable { get; set; } = false;
        public bool isSold { get; set; } = false;
        public string OwnerId { get; set; }
        public virtual Category CategoryParentNavigation { get; set; }
        public virtual AppUser UserParentNavigation { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
    }
}
