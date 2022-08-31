using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrunKatalogProjesi.Service.Validations;

namespace UrunKatalogProjesi.API.StartupExtensions
{
    public static class ExtensionValidations
    {
        public static void AddValidations(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<RegisterValidation>());
            services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<LoginValidation>());
            services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CategoryValidation>());
            //services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductValidation>());

        }
    }
}
