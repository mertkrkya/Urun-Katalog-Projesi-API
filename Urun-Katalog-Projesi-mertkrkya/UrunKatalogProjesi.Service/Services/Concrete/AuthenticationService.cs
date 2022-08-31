using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Entities;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.UnitofWork;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Data.Repositories;

namespace UrunKatalogProjesi.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitofWork _unitofWork;
        private readonly UserManager<AppUser> userManager;
        private readonly IBaseRepository<AccountRefreshToken> _refreshTokenRepository;
        public AuthenticationService(ITokenService tokenService, IUnitofWork unitofWork, IBaseRepository<AccountRefreshToken> refreshTokenRepository,
            IAccountRepository accountRepository, UserManager<AppUser> userManager)
        {
            _tokenService = tokenService;
            _unitofWork = unitofWork;
            _refreshTokenRepository = refreshTokenRepository;
            _accountRepository = accountRepository;
            this.userManager = userManager;
        }
        public async Task<ResponseEntity> CreateTokenAsync(AppUser appUser)
        {
            if (appUser == null)
                return new ResponseEntity("User cannot found");
            var token = _tokenService.CreateToken(appUser);
            var accountRefreshToken = await _refreshTokenRepository.Find(r => r.UserId == appUser.Id).FirstOrDefaultAsync();
            if (accountRefreshToken == null)
            {
                await _refreshTokenRepository.InsertAsync((new AccountRefreshToken()
                {
                    UserId = appUser.Id,
                    Code = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration
                }));
            }
            else
            {
                accountRefreshToken.Code = token.RefreshToken;
                accountRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitofWork.CommitAsync();
            return new ResponseEntity(token);
        }

        public async Task<ResponseEntity> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var accountRefreshToken = await _refreshTokenRepository.Find(r => r.Code == refreshToken).FirstOrDefaultAsync();
            if (accountRefreshToken == null)
                return new ResponseEntity("Refresh Token cannot found .");
            var existAccount = await _accountRepository.Find(r => r.Id == accountRefreshToken.UserId).FirstOrDefaultAsync();
            if (existAccount == null)
                return new ResponseEntity("Invalid Account Id");
            var token = _tokenService.CreateToken(existAccount);

            accountRefreshToken.Code = token.RefreshToken;
            accountRefreshToken.Expiration = token.RefreshTokenExpiration;
            await _unitofWork.CommitAsync();
            return new ResponseEntity(token);
        }
    }
}
