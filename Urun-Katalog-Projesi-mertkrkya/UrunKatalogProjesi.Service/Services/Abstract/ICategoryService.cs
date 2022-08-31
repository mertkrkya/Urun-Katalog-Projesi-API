using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Data.Models;

namespace UrunKatalogProjesi.Service.Services.Abstract
{
    public interface ICategoryService : IBaseService<CategoryDto, Category>
    {
    }
}
