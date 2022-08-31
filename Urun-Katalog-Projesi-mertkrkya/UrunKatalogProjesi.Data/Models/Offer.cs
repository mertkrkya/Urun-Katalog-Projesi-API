using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Entities;

namespace UrunKatalogProjesi.Data.Models
{
    public class Offer : Entity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string OfferUserId { get; set; }
        public OfferStatuses OfferStatus { get; set; }
        public decimal OfferPrice { get; set; } //Fiyat belirtme için.
        public decimal OfferPercent { get; set; } //Yüzdelik için.
        public virtual AppUser UserParentNavigation { get; set; }
        public virtual Product ProductParentNavigation { get; set; }

    }
}
