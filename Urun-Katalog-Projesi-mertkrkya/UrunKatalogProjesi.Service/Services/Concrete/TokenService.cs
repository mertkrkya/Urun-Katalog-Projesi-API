using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UrunKatalogProjesi.Data.Configurations;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Dto;

namespace UrunKatalogProjesi.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly UserManager<AppUser> userManager;

        public TokenService(UserManager<AppUser> userManager, IOptions<JwtConfig> options)
        {
            _jwtConfig = options.Value;
            this.userManager = userManager;
        }

        public JwtTokenDto CreateToken(AppUser appUser)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_jwtConfig.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_jwtConfig.RefreshTokenExpiration);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature); //İmzayı oluşturuyoruz.
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaims(appUser),
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(jwtSecurityToken);
            JwtTokenDto jwtToken = new JwtTokenDto();
            jwtToken.AccessToken = token;
            jwtToken.AccessTokenExpiration = accessTokenExpiration;
            jwtToken.RefreshToken = CreateRefreshToken();
            jwtToken.RefreshTokenExpiration = refreshTokenExpiration;
            return jwtToken;
        }
        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];
            var rndNumber = RandomNumberGenerator.Create();
            rndNumber.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        private IEnumerable<Claim> GetClaims(AppUser appUser)
        {
            string phoneNumber = "";
            if (!string.IsNullOrWhiteSpace(appUser.PhoneNumber))
                phoneNumber = appUser.PhoneNumber;
            var claimList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.Email, appUser.Email),
                new Claim(ClaimTypes.MobilePhone, phoneNumber),
                new Claim("UserId", appUser.Id.ToString())
            };
            return claimList;
        }
    }
}
