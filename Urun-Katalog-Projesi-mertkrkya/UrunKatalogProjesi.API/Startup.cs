using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using UrunKatalogProjesi.API.Filters;
using UrunKatalogProjesi.API.Middleware;
using UrunKatalogProjesi.API.StartupExtensions;

namespace UrunKatalogProjesi.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add(new ValidateFilter())).AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddContextDependencyInjection(Configuration);
            services.AddServicesDependencyInjection();
            services.AddValidations();
            services.AddCustomizeSwagger();
            ExtensionService.CheckFileDirectories(Configuration); //Sunucu her ayaða kalktýðý zaman dosyalarýn varlýðýný kontrol eder. Dosyalar yoksa oluþturur.
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.CustomExceptionMiddleware();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/UrunKatalogProject/swagger.json", "UrunKatalogProjesi.API v1");
                options.DefaultModelsExpandDepth(-1);
            });
            app.UseHangfireDashboard("/hangfire");
        }
    }
}
