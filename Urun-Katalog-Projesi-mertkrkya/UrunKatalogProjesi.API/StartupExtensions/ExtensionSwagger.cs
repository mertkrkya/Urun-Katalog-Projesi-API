using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using UrunKatalogProjesi.API.Filters;

namespace UrunKatalogProjesi.API.StartupExtensions
{
    public static class ExtensionSwagger
    {
        public static void AddCustomizeSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(gen =>
            {
                gen.SwaggerDoc("UrunKatalogProject", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "V1",
                    Title = "Urun Katalog API",
                });
                gen.OperationFilter<SwaggerFileOperationFilter>();
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Protein Management for IT Company",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                gen.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                gen.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
            });
        }
    }
}
