using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;
using UrunKatalogProjesi.Service.Services.Abstract;

namespace UrunKatalogProjesi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManagementController : ControllerBase
    {
        private readonly IManagementService _managementService;
        public ManagementController(IManagementService managementService)
        {
            _managementService = managementService;
        }
        [HttpPost("ImageUpload")]
        public async Task<IActionResult> ImageUpload([FromForm] IFormFile file)
        {
            var result = await _managementService.UploadPhoto(file);

            if (!result.isSuccess)
                return BadRequest(result);
            Log.Information($"{User.Identity?.Name} is image upload.");
            return Ok(result);
        }
        [HttpGet("GetAllConfigData")]
        public async Task<IActionResult> GetAllConfigData()
        {
            var result = await _managementService.GetAllConfigData();

            if (!result.isSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
