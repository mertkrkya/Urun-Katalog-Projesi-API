using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Data.Entities;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Repositories.Abstract;
using UrunKatalogProjesi.Data.UnitofWork;
using UrunKatalogProjesi.Service.Services.Abstract;

namespace UrunKatalogProjesi.Service.Services.Concrete
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitofWork _unitofWork;
        protected readonly IMapper _mapper;
        private readonly UserManager<AppUser> userManager;
        public OfferService(IOfferRepository offerRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor,
            IUnitofWork unitofWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _offerRepository = offerRepository;
            _productRepository = productRepository;
            _unitofWork = unitofWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<ResponseEntity> MakeAnOffer(InsertOfferDto offer)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(offer.ProductId);
                if (product == null)
                    return new ResponseEntity("Product cannot found. Product ID:" + offer.ProductId);
                if (!product.isOfferable)
                    return new ResponseEntity("This product is not offerable");
                if (product.isSold)
                    return new ResponseEntity("This product was sold.");
                if (offer.OfferPercent > 0)
                    offer.OfferPrice = (offer.OfferPercent * product.Price) / 100;
                var currentUser = _httpContextAccessor.HttpContext.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                var offerExist = _offerRepository.Find(r => r.ProductId == offer.ProductId && r.OfferUserId == userId).FirstOrDefault();
                if (offerExist != null)
                    return new ResponseEntity("You already have an offer for this product.");
                if (product.OwnerId == userId)
                    return new ResponseEntity("The user already owns this product.");
                var offerEntity = _mapper.Map<InsertOfferDto, Offer>(offer);
                offerEntity.OfferStatus = OfferStatuses.Wait;
                offerEntity.OfferUserId = userId;
                offerEntity.CreatedBy = userId;
                await _offerRepository.InsertAsync(offerEntity);
                await _unitofWork.CommitAsync();
                var mapOffer = _mapper.Map<Offer, OfferDto>(offerEntity);
                return new ResponseEntity(mapOffer);
            }
            catch (Exception e)
            {

                throw new Exception("Make An Offer Save Error. Message:" + e.Message);
            }

        }
        public async Task<ResponseEntity> BuyProduct(int productId)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                    return new ResponseEntity("Product cannot found. Product ID:" + productId);
                if (!product.isOfferable)
                    return new ResponseEntity("This product is not offerable");
                if (product.isSold)
                    return new ResponseEntity("This product was sold.");
                var currentUser = _httpContextAccessor.HttpContext.User.Claims.Where(r => r.Type == "UserId").FirstOrDefault();
                if (currentUser == null)
                    return new ResponseEntity("User cannot be null");
                var userId = currentUser.Value;
                if (product.OwnerId == userId)
                    return new ResponseEntity("The user already owns the product.");
                product.OwnerId = userId;
                product.isSold = true;
                var productOwnerUser = await userManager.FindByIdAsync(product.OwnerId);
                var mailData = new MailDataDto();
                mailData.ProductName = product.ProductName;
                mailData.Price = product.Price;
                _productRepository.Update(product);
                _unitofWork.Commit();
                BackgroundJob.Jobs.FireAndForgetJobs.EmailSendJob(EmailTypes.Sold, productOwnerUser.UserName, productOwnerUser.Email, mailData);
                return new ResponseEntity(data: "The product was successfully bought.");
            }
            catch (Exception e)
            {
                throw new Exception("Buy Product Save Error. Message:" + e.Message);
            }
        }

        public async Task<ResponseEntity> CancelOffer(int id)
        {
            try
            {
                var offer = await _offerRepository.GetByIdAsync(id);
                if(offer == null)
                    return new ResponseEntity("This offer cannot found. Offer ID:" + id);
                if(offer.OfferStatus != OfferStatuses.Wait)
                    return new ResponseEntity("This offer cannot be cancel.");
                offer.OfferStatus = OfferStatuses.Cancel;
                var currentUser = _httpContextAccessor.HttpContext.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                var currentTime = DateTime.UtcNow;
                offer.ModifiedBy = userId;
                offer.ModifiedDate = currentTime;
                _offerRepository.Update(offer);
                _unitofWork.Commit();
                var product = await _productRepository.GetByIdAsync(offer.ProductId);
                var productOwnerUser = await userManager.FindByIdAsync(product.OwnerId);
                var mailData = new MailDataDto();
                mailData.ProductName = product.ProductName;
                mailData.Price = offer.OfferPrice;
                BackgroundJob.Jobs.FireAndForgetJobs.EmailSendJob(EmailTypes.UnOffer, productOwnerUser.UserName, productOwnerUser.Email, mailData);
                return new ResponseEntity(data: "The offer was successfully canceled");
            }
            catch (Exception e)
            {
                throw new Exception("Cancel Offer Save Error. Message:" + e.Message);
            }
        }

        public async Task<ResponseEntity> UpdateOffer(int id, InsertOfferDto offer)
        {
            try
            {
                var dbOffer = await _offerRepository.GetByIdAsync(id);
                if (dbOffer == null)
                    return new ResponseEntity("Offer cannot found. Offer ID:" + id);
                var product = await _productRepository.GetByIdAsync(dbOffer.ProductId);
                if (product == null)
                    return new ResponseEntity("Product cannot found. Product ID:" + offer.ProductId);
                if (offer.OfferPercent > 0)
                    dbOffer.OfferPrice = (offer.OfferPercent * product.Price) / 100;
                else
                    dbOffer.OfferPrice = offer.OfferPrice;
                var currentUser = _httpContextAccessor.HttpContext.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                var currentTime = DateTime.UtcNow;
                dbOffer.ModifiedBy = userId;
                dbOffer.ModifiedDate = currentTime;
                _offerRepository.Update(dbOffer);
                _unitofWork.Commit();
                var mapOffer = _mapper.Map<Offer, OfferDto>(dbOffer);
                return new ResponseEntity(mapOffer);
            }
            catch (Exception e)
            {

                throw new Exception("Offer Update Error. Message:" + e.Message);
            }
        }
    }
}
