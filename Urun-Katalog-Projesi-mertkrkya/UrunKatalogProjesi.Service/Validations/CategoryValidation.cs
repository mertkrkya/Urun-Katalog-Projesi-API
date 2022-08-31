using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Data.Repositories;

namespace UrunKatalogProjesi.Service.Validations
{
    public class CategoryValidation : AbstractValidator<CategoryDto>
    {
        public CategoryValidation()
        {

            RuleFor(r => r.CategoryName)
                .NotNull().WithMessage("CategoryName cannot be null.")
                .NotEmpty().WithMessage("CategoryName cannot be empty.");

        }
    }
}
