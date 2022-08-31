using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Entities;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Dto;

namespace UrunKatalogProjesi.Service.Services
{
    public interface IAccountService : IBaseService<AppUserDto,AppUser>
    {
        Task<ResponseEntity> LoginProcess(LoginRequest loginRequest);
        Task<ResponseEntity> GetMyOffers(string key, int pageNum, int pageSize);
        Task<ResponseEntity> GetMyProducts(string key, int pageNum, int pageSize);
        Task<ResponseEntity> GetMyProductOffers();
        Task<ResponseEntity> AcceptOffer(int offerId);
        Task<ResponseEntity> RejectOffer(int offerId);

    }
}
