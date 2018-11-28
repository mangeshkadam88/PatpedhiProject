using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patpedhi.Infrastructure.Data;
using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.Interfaces;
using Patpedhi.Web.Services;
using PatPedhi.Core.Interfaces;

namespace Patpedhi.Web
{
    public class Startup
    {
        private IServiceCollection _services;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // use in-memory database
            ConfigureLocalDatabases(services);

            // use real database
            // ConfigureProductionServices(services);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<PatpedhiContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            services.AddScoped<IUserProfileService, UserProfileService>();

            // Add memory cache services
            services.AddMemoryCache();

            services.Configure<CookieTempDataProviderOptions>(options => {
                options.Cookie.IsEssential = true;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateFormatString = "dd/MM/yyyy";
                })                
                ;

            _services = services;
        }

        private void ConfigureLocalDatabases(IServiceCollection services)
        {
            // use in-memory database
            services.AddDbContext<PatpedhiContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("SQLDBConnectionLocal"),
                x => x.MigrationsAssembly("Patpedhi.Infrastructure")));
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<PatpedhiContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("SQLDBConnectionProduction")));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseAuthentication();
            app.UseCookiePolicy();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=SignIn}");
            });
        }
    }
}
