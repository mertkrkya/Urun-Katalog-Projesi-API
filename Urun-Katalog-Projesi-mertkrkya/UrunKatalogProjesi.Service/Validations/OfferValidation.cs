using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Data.Repositories.Abstract;

namespace UrunKatalogProjesi.Service.Validations
{
    public class InsertOfferValidation : AbstractValidator<InsertOfferDto>
    {
        private readonly IProductRepository _productRepository;
        public InsertOfferValidation(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            this.CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => new { x.OfferPrice, x.OfferPercent }).Must(m =>
              {
                  if (m.OfferPercent == 0 && m.OfferPrice == 0)
                      return false;
                  return true;
              }).WithMessage("OfferPercent or OfferPrice must be entered");
            RuleFor(r => r.OfferPercent).GreaterThanOrEqualTo(0).WithMessage($"OfferPercent must be between 1 to 100");
            RuleFor(r => r.ProductId).Must(m =>
            {
                if (_productRepository.GetByIdAsync(m).GetAwaiter().GetResult() == null)
                    return false;
                return true;
            }).WithMessage("This product is not exist.");
            RuleFor(r => new { r.OfferPrice, r.ProductId }).Must(m =>
            {
                var product = _productRepository.GetByIdAsync(m.ProductId).GetAwaiter().GetResult();
                if(product != null)
                {
                    if (m.OfferPrice > product.Price)
                        return false;
                }
                return true;
            }).WithMessage($"OfferPrice cannot higher than product price.");
        }
    }
    public class BuyOfferValidation : AbstractValidator<BuyOfferDto>
    {
        private readonly IProductRepository _productRepository;
        public BuyOfferValidation(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            RuleFor(r => r.ProductId).Must(m =>
            {
                if (_productRepository.GetByIdAsync(m).GetAwaiter().GetResult() == null)
                    return false;
                return true;
            }).WithMessage("This product is not exist.");
            RuleFor(r => r.OfferStatus).IsInEnum().WithMessage("This offer status is not exist");
        }
    }
}
