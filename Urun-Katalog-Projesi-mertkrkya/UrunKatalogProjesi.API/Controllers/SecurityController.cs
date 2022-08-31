using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Entities;
using UrunKatalogProjesi.Data.Helper;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Service.Services;
using UrunKatalogProjesi.BackgroundJob.Jobs;
using Microsoft.Extensions.Logging;
using Serilog;

namespace UrunKatalogProjesi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountService _accountService;
        public SecurityController(IAuthenticationService authenticationService, IAccountService accountService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _authenticationService = authenticationService;
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginProcess(loginRequest);
                if (!result.isSuccess)
                {
                    return BadRequest(result);
                }
                var appUser = (AppUser)result.data;
                var tokenResult = await _authenticationService.CreateTokenAsync(appUser);
                if (!tokenResult.isSuccess)
                    return BadRequest(tokenResult);
                Log.Information($"{User.Identity?.Name} is login.");
                return Ok(tokenResult);
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDto input)
        {
            if (ModelState.IsValid)
            {
                var newUser = new AppUser
                {
                    UserName = input.UserName,
                    Email = input.Email,
                    EmailConfirmed = true,
                    TwoFactorEnabled = false
                };

                var registerUser = await _userManager.CreateAsync(newUser, input.Password);
                if (registerUser.Succeeded)
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    var user = await _userManager.FindByNameAsync(newUser.UserName);
                    var result = await _authenticationService.CreateTokenAsync(user);
                    if (!result.isSuccess)
                        return BadRequest(result);
                    FireAndForgetJobs.EmailSendJob(EmailTypes.Welcome, user.UserName, user.Email);
                    return Ok(result);
                }
                return BadRequest(new ResponseEntity(registerUser.Errors.ErrorToString()));
            }
            return BadRequest(ModelState);

        }
    }
}
