using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Dto;

namespace UrunKatalogProjesi.Service.Services
{
    public interface ITokenService
    {
        JwtTokenDto CreateToken(AppUser appUser);
    }
}
