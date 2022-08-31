using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Configurations;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Data.Entities;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Repositories.Abstract;
using UrunKatalogProjesi.Service.Services.Abstract;
using UrunKatalogProjesi.Service.Validations;

namespace UrunKatalogProjesi.Service.Services.Concrete
{
    public class ManagementService : IManagementService
    {
        private readonly IHostingEnvironment _env;
        private readonly SystemOptionConfig userOptionsConfig;
        private readonly IConfigRepository<Brand> _brandConfigRepository;
        private readonly IConfigRepository<Color> _colorConfigRepository;
        public ManagementService(IHostingEnvironment env, IOptions<SystemOptionConfig> options,
            IConfigRepository<Brand> brandConfigRepository, IConfigRepository<Color> colorConfigRepository)
        {
            _env = env;
            userOptionsConfig = options.Value;
            _brandConfigRepository = brandConfigRepository;
            _colorConfigRepository = colorConfigRepository;
        }

        public async Task<ResponseEntity> GetAllConfigData()
        {
            ConfigDataDto configModels = new ConfigDataDto();
            var userStatuses = Enum.GetValues(typeof(UserStatuses)).Cast<UserStatuses>().Select(r => new GenericConfigModel
            {
                Id = r.ToString(),
                Name = r.ToString(),
                Order = 0
            }).ToList();
            var categoryStatuses = Enum.GetValues(typeof(CategoryStatuses)).Cast<CategoryStatuses>().Select(r => new GenericConfigModel
            {
                Id = r.ToString(),
                Name = r.ToString(),
                Order = 0
            }).ToList();
            var offerStatuses = Enum.GetValues(typeof(OfferStatuses)).Cast<OfferStatuses>().Select(r => new GenericConfigModel
            {
                Id = r.ToString(),
                Name = r.ToString(),
                Order = 0
            }).ToList();
            var productStatuses = Enum.GetValues(typeof(ProductStatuses)).Cast<ProductStatuses>().Select(r => new GenericConfigModel
            {
                Id = r.ToString(),
                Name = r.ToString(),
                Order = 0
            }).ToList();
            configModels.UserStatuses = userStatuses;
            configModels.CategoryStatuses = categoryStatuses;
            configModels.ProductStatuses = productStatuses;
            configModels.OfferStatuses = offerStatuses;
            configModels.Brand = await _brandConfigRepository.GetAllAsync();
            configModels.Color = await _colorConfigRepository.GetAllAsync();
            return new ResponseEntity(configModels);
        }

        public async Task<ResponseEntity> UploadPhoto([FromForm] IFormFile image)
        {
            int maxFileSize = Convert.ToInt32(userOptionsConfig.MaxFileKbSize);
            var validateResult = FileValidation.ImageValidation(image, maxFileSize);
            if (!string.IsNullOrWhiteSpace(validateResult))
                return new ResponseEntity(validateResult);
            try
            {
                var docName = Path.GetFileName(image.FileName);
                var fileName = userOptionsConfig.ImportPhotoDirectory + "/" + DateTime.Now.ToString("Mddyyyyhhmmss") + docName;
                FileStream fs = new FileStream(fileName, FileMode.Create);
                image.CopyTo(fs);
                fs.Close();
                return new ResponseEntity(new FileResponse(fileName));
            }
            catch (Exception e)
            {
                throw new Exception("File Upload Error. Message: " + e.Message);
            }
        }
        public class FileResponse
        {
            public string FileLink { get; set; }
            public FileResponse(string fileLink)
            {
                FileLink = fileLink;
            }
        }
    }
}
