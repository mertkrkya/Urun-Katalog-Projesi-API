using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Repositories;
using UrunKatalogProjesi.Service.Services;

namespace UrunKatalogProjesi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost]
        [Route("AcceptOffer")]
        public async Task<IActionResult> AcceptOffer(int offerId)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.AcceptOffer(offerId);
                if (!result.isSuccess)
                {
                    return BadRequest(result);
                }
                Log.Information($"{User.Identity?.Name} is accept offer. Offer ID: {offerId}.");
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        [Route("RejectOffer")]
        public async Task<IActionResult> RejectOffer(int offerId)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RejectOffer(offerId);
                if (!result.isSuccess)
                {
                    return BadRequest(result);
                }
                Log.Information($"{User.Identity?.Name} is reject offer. Offer ID: {offerId}.");
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        [Route("GetMyOffers")]
        public async Task<IActionResult> GetMyOffers([FromQuery] string key, [FromQuery] int pageNum, [FromQuery] int pageSize)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.GetMyOffers(key,pageNum,pageSize);
                if (!result.isSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        [Route("GetMyProductOffers")]
        public async Task<IActionResult> GetMyProductOffers()
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.GetMyProductOffers();
                if (!result.isSuccess)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        [Route("GetMyProducts")]
        public async Task<IActionResult> GetMyProducts([FromQuery] string key, [FromQuery] int pageNum, [FromQuery] int pageSize)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.GetMyProducts(key,pageNum,pageSize);
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
