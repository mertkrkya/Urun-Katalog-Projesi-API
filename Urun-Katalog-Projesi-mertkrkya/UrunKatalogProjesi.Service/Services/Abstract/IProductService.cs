using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Data.Entities;
using UrunKatalogProjesi.Data.Models;

namespace UrunKatalogProjesi.Service.Services.Abstract
{
    public interface IProductService : IBaseService<ProductDto, Product>
    {
    }
}
