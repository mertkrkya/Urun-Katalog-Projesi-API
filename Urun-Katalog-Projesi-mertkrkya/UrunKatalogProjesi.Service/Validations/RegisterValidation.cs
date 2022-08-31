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
    public class RegisterValidation : AbstractValidator<RegisterDto>
    {
        public RegisterValidation()
        {

            RuleFor(r => r.UserName)
                .NotNull().WithMessage("UserName cannot be null.")
                .NotEmpty().WithMessage("UserName cannot be empty.");
            

            RuleFor(r => r.Password)
                .NotNull().WithMessage("Password cannot be null.")
                .NotEmpty().WithMessage("Password cannot be empty.")
                .Length(8, 20).WithMessage("Password length must be between 8 to 20 char long.");

            RuleFor(r => r.Email)
                .NotNull().WithMessage("Email cannot be null.")
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Email must be valid.");
        }
    }
}
