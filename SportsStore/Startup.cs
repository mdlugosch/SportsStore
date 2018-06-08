using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;

namespace SportsStore
{
    public class Startup
    {
        #region Beschreibung
        /*
         * Um dem Dienstontainer weiter unten im Code ein ConnectionString übergeben zu können
         * ist ein Object des Interfaces IConfiguration notwendig in die der ConnectionString
         * eingetragen werden kann. Spöter wird dann die Configuration Property dem Dienstcontainer
         * als einer der Parameter übergeben.
         */
        #endregion
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;    

        // DI-Container
        public void ConfigureServices(IServiceCollection services)
        {
            #region Beschreibung
            /*
             * Hinzufügen eines DbContext von Typ ApplicationDbContext zu einem
             * Dienstcontainer. Dem DbContext wird dabei ein ConnectionString zur
             * Datenbank übergeben der aus der appsettings.json ausgelesen wird.
             */
            #endregion
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                Configuration["Data:SportStoreProducts:ConnectionString"]));

            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(
                Configuration["Data:SportStoreIdentity:ConnectionString"]));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            #region Beschreibung
            /*
             * Überall wo das Interface IProductRepository verwendet wird, wird ein
             * Object der Klasse EFProductRepository geliefert. AddTransient sorgt
             * dafür das jedesmal wenn ein Interface angefragt wird ein neues Object
             * der angegebenen Klasse erzeugt wird.
             */
            #endregion
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        #region Beschreibung
        /*
         * UseDeveloperExceptionPage Sorgt für detailierte Fehlerseiten im Browser
         * und sollte nur in der Entwicklungsphase genutzt werden.
         * 
         * UseStatusCodePages ermöglich Stadart-HTTP-Antworten benutzerdefiniert zu gesatalten.
         * 
         * UseStaticFiles ermöglicht die nutzung von Statischen Inhalten aus dem wwwroot-Ordner.
         * 
         * UseMvc aktiviert ASP.NET Core MVC Funktionallität.
         */
        #endregion
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        { // Test
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            } else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: null,
                template: "{category}/Page{productPage:int}",
                defaults: new { Controller = "Product", action = "List" });

                routes.MapRoute(
                name: null,
                template: "{category}/Page{productPage:int}",
                defaults: new { Controller = "Product", action = "List", productPage = 1 });

                routes.MapRoute(
                name: null,
                template: "{category}",
                defaults: new { Controller = "Product", action = "List", productPage = 1 });

                routes.MapRoute(
                name: null,
                template: "",
                defaults: new { Controller = "Product", action = "List", productPage = 1 });

                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });
            // SeedData.EnsurePopulated(app);
            // IdentitySeedData.EnsurePopulated(app);
        }
    }
}
