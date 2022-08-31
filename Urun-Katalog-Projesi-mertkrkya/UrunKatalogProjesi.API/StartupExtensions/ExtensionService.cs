using AutoMapper;
using Hangfire;
using Hangfire.PostgreSql;
using JUrunKatalogProjesi.Core.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using UrunKatalogProjesi.BackgroundJob.Services.Abstract;
using UrunKatalogProjesi.BackgroundJob.Services.Concrete;
using UrunKatalogProjesi.Data.Configurations;
using UrunKatalogProjesi.Data.Context;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Repositories;
using UrunKatalogProjesi.Data.Repositories.Abstract;
using UrunKatalogProjesi.Data.Repositories.Concrete;
using UrunKatalogProjesi.Data.UnitofWork;
using UrunKatalogProjesi.Service.Mapper;
using UrunKatalogProjesi.Service.Services;
using UrunKatalogProjesi.Service.Services.Abstract;
using UrunKatalogProjesi.Service.Services.Concrete;

namespace UrunKatalogProjesi.API.StartupExtensions
{
    public static class ExtensionService
    {
        public static void AddContextDependencyInjection(this IServiceCollection services, IConfiguration Configuration)
        {
            // cors 
            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ConfigDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfire(config => config.UsePostgreSqlStorage(Configuration.GetConnectionString("DefaultConnection")));
            AppDbContext.SetContextConnectionString(Configuration.GetConnectionString("DefaultConnection"));
            ConfigDbContext.SetContextConnectionString(Configuration.GetConnectionString("DefaultConnection"));
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            services.Configure<SystemOptionConfig>(Configuration.GetSection("SystemOptionConfig"));
            services.Configure<EmailConfig>(Configuration.GetSection("EmailConfig"));
            // identity
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddHangfireServer();


            // identity options
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 1;
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            {
                var JwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, // default True
                    ValidIssuer = JwtConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtConfig.Secret)),
                    ValidAudience = JwtConfig.Audience,
                    ValidateAudience = false, // default True
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });
        }
        public static void AddServicesDependencyInjection(this IServiceCollection services)
        {
            // add service
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IConfigRepository<>), typeof(ConfigRepository<>));
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IManagementService, ManagementService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IUnitofWork, UnitOfWork>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
            // mapper 
            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
        public static void CheckFileDirectories(IConfiguration Configuration)
        {
            var importPhotoDirectory = Configuration.GetSection("SystemOptionConfig").GetValue<string>("ImportPhotoDirectory");
            if (!System.IO.Directory.Exists(importPhotoDirectory))
                System.IO.Directory.CreateDirectory(importPhotoDirectory);
        }
    }
}
