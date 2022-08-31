using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Data.Entities;

namespace UrunKatalogProjesi.Service.Services.Abstract
{
    public interface IOfferService
    {
        /// <summary>
        /// MakeAnOffer metodunda offer yapılacak.
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        Task<ResponseEntity> MakeAnOffer(InsertOfferDto offer);
        /// <summary>
        /// BuyProduct metodunda product direkt satın alınacak.
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        Task<ResponseEntity> BuyProduct(int productId);
        /// <summary>
        /// CancelOffer metodunda offer iptal edilebilecek.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseEntity> CancelOffer(int id);
        /// <summary>
        /// UpdateOffer metodunda offer durumu değiştirilecek. Yani kullanıcı reddedecek veya kabul edecek.
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        Task<ResponseEntity> UpdateOffer(int id, InsertOfferDto offer);

    }
}
