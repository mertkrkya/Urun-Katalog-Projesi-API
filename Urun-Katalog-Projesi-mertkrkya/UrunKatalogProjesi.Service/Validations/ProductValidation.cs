using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Repositories;
using UrunKatalogProjesi.Data.Repositories.Abstract;

namespace UrunKatalogProjesi.Service.Validations
{
    public class ProductValidation : AbstractValidator<ProductDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfigRepository<Brand> _brandConfigRepository;
        private readonly IConfigRepository<Color> _colorConfigRepository;
        public ProductValidation(ICategoryRepository categoryRepository, IConfigRepository<Brand> brandConfigRepository, IConfigRepository<Color> colorConfigRepository)
        {
            _categoryRepository = categoryRepository;
            _brandConfigRepository = brandConfigRepository;
            _colorConfigRepository = colorConfigRepository;

            RuleFor(r => r.ProductName)
                .NotNull().WithMessage("ProductName cannot be null.")
                .NotEmpty().WithMessage("ProductName cannot be empty.")
                .MaximumLength(100).WithMessage("ProductName char length cannot be more than 100.");

            RuleFor(r => r.Description)
                .NotNull().WithMessage("Description cannot be null.")
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MaximumLength(500).WithMessage("Description char length cannot be more than 500.");

            RuleFor(r => r.Price)
                .InclusiveBetween(1, decimal.MaxValue).WithMessage($"Price must be between 1 to {decimal.MaxValue}");

            RuleFor(r => r.ProductStatus)
                .IsInEnum().WithMessage("This ProductStatus cannot exist.");

            RuleFor(r => r.CategoryId).Must(r => 
                {
                    if (_categoryRepository.GetByIdAsync(r).GetAwaiter().GetResult() == null)
                        return false;
                    return true;
                }).WithMessage("This CategoryId cannot exist.");

            RuleFor(r => r.Brand).Must(r =>
            {
                if (string.IsNullOrEmpty(r))
                    return true;
                if (_brandConfigRepository.GetByIdAsync(r).GetAwaiter().GetResult() == null)
                    return false;
                return true;
            }).WithMessage("This Brand is not exist.");

            RuleFor(r => r.Color).Must(r =>
            {
                if (string.IsNullOrEmpty(r))
                    return true;
                if (_colorConfigRepository.GetByIdAsync(r).GetAwaiter().GetResult() == null)
                    return false;
                return true;
            }).WithMessage("This Color is not exist.");

            RuleFor(r => r.ImageLink).Must(r =>
            {
                if (File.Exists(r))
                    return true;
                return false;
            }).WithMessage("This Image is not exist.");
        }
    }
}
