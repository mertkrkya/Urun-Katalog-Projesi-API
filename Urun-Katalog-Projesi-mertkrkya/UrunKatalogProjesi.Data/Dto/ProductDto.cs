using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Entities;
using UrunKatalogProjesi.Data.Models;

namespace UrunKatalogProjesi.Data.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string ImageLink { get; set; }
        public ProductStatuses ProductStatus { get; set; }
        public decimal Price { get; set; }
        public bool isOfferable { get; set; } = false;
        public bool isSold { get; set; } = false;
    }
    public class DetailProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string ImageLink { get; set; }
        public ProductStatuses ProductStatus { get; set; }
        public decimal Price { get; set; }
        public bool isOfferable { get; set; } = false;
        public bool isSold { get; set; } = false;
        public string OwnerId { get; set; }
        public ICollection<OfferDto> Offers { get; set; }
    }
}
