using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Entities;

namespace UrunKatalogProjesi.Service.Services.Abstract
{
    public interface IManagementService
    {
        Task<ResponseEntity> UploadPhoto([FromForm] IFormFile image);
        Task<ResponseEntity> GetAllConfigData();
    }
}
