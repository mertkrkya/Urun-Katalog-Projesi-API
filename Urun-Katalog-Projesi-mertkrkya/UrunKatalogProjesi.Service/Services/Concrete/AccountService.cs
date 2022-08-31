using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Configurations;
using UrunKatalogProjesi.Data.Entities;
using UrunKatalogProjesi.Data.Helper;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.UnitofWork;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Data.Repositories;
using Microsoft.AspNetCore.Http;
using UrunKatalogProjesi.Data.Repositories.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace UrunKatalogProjesi.Service.Services
{
    public class AccountService : BaseService<AppUserDto,AppUser>, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly SystemOptionConfig userOptionsConfig;
        private readonly IUnitofWork _unitofWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;
        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, 
            IUnitofWork unitofWork, IAccountRepository accountRepository, IOfferRepository offerRepository, IProductRepository productRepository,
            IOptions<SystemOptionConfig> options, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache) : base(accountRepository,unitofWork,mapper,httpContextAccessor)
        {
            _accountRepository = accountRepository;
            this.userManager = userManager;
            userOptionsConfig = options.Value;
            _unitofWork = unitofWork;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _offerRepository = offerRepository;
            _productRepository = productRepository;
            _memoryCache = memoryCache;
        }

        public async Task<ResponseEntity> LoginProcess(LoginRequest loginRequest)
        {
            var appUser = await userManager.FindByNameAsync(loginRequest.UserName);
            if (appUser == null)
                return new ResponseEntity("You have entered an invalid username or password.");
            if(appUser.UserStatus == UserStatuses.Block)
            {
                return new ResponseEntity("This account has been blocked.");
            }
            var loginResult = await _signInManager.PasswordSignInAsync(loginRequest.UserName, loginRequest.Password, false, false);
            if(loginResult.Succeeded)
            {
                await userManager.ResetAccessFailedCountAsync(appUser);
                return new ResponseEntity(appUser);
            }
            var result = await userManager.AccessFailedAsync(appUser);
            if(!result.Succeeded)
            {
                return new ResponseEntity("Access Failed Error. Error: "+ result.Errors.ErrorToString());
            }
            int failCount = await userManager.GetAccessFailedCountAsync(appUser);
            if(failCount == userOptionsConfig.BlockAccessFailedCount)
            {
                appUser.UserStatus = UserStatuses.Block;
                try
                {
                    _unitofWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Login Failed Process Commit Fail. Error :" + ex.Message);
                }
                BackgroundJob.Jobs.FireAndForgetJobs.EmailSendJob(EmailTypes.Block, appUser.UserName, appUser.Email);
                return new ResponseEntity("The account status is block.");
            }
            else
            {
                return new ResponseEntity($"You have entered an invalid username or password. You have {userOptionsConfig.BlockAccessFailedCount - failCount} last entries left for successful login.");
            }
        }
        public async Task<ResponseEntity> AcceptOffer(int offerId)
        {
            try
            {
                var offer = await _offerRepository.GetByIdAsync(offerId);
                if (offer == null)
                    return new ResponseEntity("This offer cannot exist. Offer ID: " + offerId);
                offer.OfferStatus = OfferStatuses.Accept;
                var product = await _productRepository.GetByIdAsync(offer.ProductId);
                var currentUser = _httpContextAccessor.HttpContext.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await userManager.FindByIdAsync(offer.OfferUserId);
                var mailData = new MailDataDto();
                mailData.ProductName = product.ProductName;
                mailData.Price = offer.OfferPrice;
                offer.ModifiedDate = DateTime.UtcNow;
                offer.ModifiedBy = userId;
                product.OwnerId = offer.OfferUserId;
                _productRepository.Update(product);
                _offerRepository.Update(offer);
                _unitofWork.Commit();
                BackgroundJob.Jobs.FireAndForgetJobs.EmailSendJob(EmailTypes.AcceptOffer, user.UserName, user.Email, mailData);
                return new ResponseEntity("The offer was successfully accept.");
            }
            catch (Exception e)
            {
                throw new Exception("Accept Offer Save Error. Message:" + e.Message);
            }
        }
        public async Task<ResponseEntity> RejectOffer(int offerId)
        {
            try
            {
                var offer = await _offerRepository.GetByIdAsync(offerId);
                if (offer == null)
                    return new ResponseEntity("This offer cannot exist. Offer ID: " + offerId);
                offer.OfferStatus = OfferStatuses.Reject;
                var product = await _productRepository.GetByIdAsync(offer.ProductId);
                var currentUser = _httpContextAccessor.HttpContext.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await userManager.FindByIdAsync(offer.OfferUserId);
                var mailData = new MailDataDto();
                mailData.ProductName = product.ProductName;
                mailData.Price = offer.OfferPrice;
                offer.ModifiedDate = DateTime.UtcNow;
                offer.ModifiedBy = userId;
                _offerRepository.Update(offer);
                _unitofWork.Commit();
                BackgroundJob.Jobs.FireAndForgetJobs.EmailSendJob(EmailTypes.RejectOffer, user.UserName, user.Email, mailData);
                return new ResponseEntity("The offer was successfully reject.");
            }
            catch (Exception e)
            {
                throw new Exception("Reject Offer Save Error. Message:" + e.Message);
            }
        }
        public async Task<ResponseEntity> GetMyOffers(string key, int pageNum, int pageSize)
        {
            try
            {
                var currentUser = _httpContextAccessor.HttpContext.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                List<Offer> offers = new List<Offer>();
                if (!_memoryCache.TryGetValue(key, out offers))
                {
                    offers = _offerRepository.Find(r => r.OfferUserId == userId).ToList();

                    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                    options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                    options.SlidingExpiration = TimeSpan.FromSeconds(10);

                    _memoryCache.Set<List<Offer>>(key, offers, options);
                }
                var mapOffers = _mapper.Map<List<Offer>, List<OfferDto>>(offers);
                var result = new PaginationResultDto<OfferDto>();
                var resultData = mapOffers.Skip((pageNum - 1) * pageNum).Take(pageSize).ToList();
                result.Items = resultData;
                result.totalItems = mapOffers.Count;
                result.currentPage = pageNum;
                return new ResponseEntity(result);
            }
            catch (Exception e)
            {
                throw new Exception("GetMyOffers Error.");
            }

        }
        public async Task<ResponseEntity> GetMyProductOffers()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var products = _productRepository.Find(r => r.OwnerId == userId).Select(r => r.Id).ToList();
            var offers = _offerRepository.Find(r => products.Contains(r.ProductId)).ToList();
            var mapOffers = _mapper.Map<List<Offer>, List<OfferDto>>(offers);
            return new ResponseEntity(mapOffers);
        }
        public async Task<ResponseEntity> GetMyProducts(string key, int pageNum, int pageSize)
        {
            try
            {
                var currentUser = _httpContextAccessor.HttpContext.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                List<Product> products = new List<Product>();
                if (!_memoryCache.TryGetValue(key, out products))
                {
                    products = _productRepository.Find(r => r.OwnerId == userId).ToList();

                    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                    options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                    options.SlidingExpiration = TimeSpan.FromSeconds(10);

                    _memoryCache.Set<List<Product>>(key, products, options);
                }
                if (pageNum < 0)
                    pageNum = 1;
                var mapProducts = _mapper.Map<List<Product>, List<ProductDto>>(products);
                var result = new PaginationResultDto<ProductDto>();
                var resultData = mapProducts.Skip((pageNum - 1) * pageNum).Take(pageSize).ToList();
                result.Items = resultData;
                result.totalItems = mapProducts.Count;
                result.currentPage = pageNum;
                return new ResponseEntity(result);
            }
            catch (Exception)
            {
                throw new Exception("GetMyProducts Error.");
            }

        }
    }
}
