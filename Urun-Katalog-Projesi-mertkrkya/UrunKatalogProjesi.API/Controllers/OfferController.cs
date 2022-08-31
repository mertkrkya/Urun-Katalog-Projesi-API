using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Service.Services.Abstract;

namespace UrunKatalogProjesi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;
        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpPost]
        [Route("MakeAnOffer")]
        public async Task<IActionResult> MakeAnOffer([FromBody] InsertOfferDto offer)
        {
            if (ModelState.IsValid)
            {
                var result = await _offerService.MakeAnOffer(offer);
                if (!result.isSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        [Route("BuyProduct")]
        public async Task<IActionResult> BuyProduct(int productId)
        {
            if (ModelState.IsValid)
            {
                var result = await _offerService.BuyProduct(productId);
                if (!result.isSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPut]
        [Route("CancelOffer")]
        public async Task<IActionResult> CancelOffer(int offerId)
        {
            if (ModelState.IsValid)
            {
                var result = await _offerService.CancelOffer(offerId);
                if (!result.isSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPut]
        [Route("UpdateOffer")]
        public async Task<IActionResult> UpdateOffer(int offerId, InsertOfferDto offer)
        {
            if (ModelState.IsValid)
            {
                var result = await _offerService.UpdateOffer(offerId, offer);
                if (!result.isSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
    }
}
