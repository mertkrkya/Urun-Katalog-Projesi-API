using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Entities;

namespace UrunKatalogProjesi.Data.Dto
{
    public class OfferDto //Gösterimlerde bu kullanılacak.
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string OfferUserId { get; set; }
        public OfferStatuses OfferStatus { get; set; }
        public decimal OfferPrice { get; set; } //Fiyat belirtme için.
        public decimal OfferPercent { get; set; } //Yüzdelik için.
    }
    public class InsertOfferDto //Insert ve Update işlemlerinde bu kullanılacak.
    {
        public int ProductId { get; set; }
        public decimal OfferPrice { get; set; } //Fiyat belirtme için.
        public decimal OfferPercent { get; set; } //Yüzdelik için.
    }
    public class BuyOfferDto //Satın almada bu model kullanılacak.
    {
        public int ProductId { get; set; }
        public OfferStatuses OfferStatus { get; set; } = OfferStatuses.Accept;
    }
}
