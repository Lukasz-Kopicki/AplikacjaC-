using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Facebook;
using IoC;
using Infrastructure.Data.Context;
using DataAccessLogic.Entities;

namespace AplikacjaFryzjer_v2
{
    public class Startup 
    {
        private IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //Add dependency injection container
            services.AddApplication();
            //Compilation in runtime
            services.AddRazorPages().AddRazorRuntimeCompilation();

            // Configure to connect with database
            services.AddDbContextPool<AppDbContext>(options => 
            options.UseSqlServer(_config.GetConnectionString("AuthDbContextConnection")));


            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllersWithViews();

            // AddIdentity registers the services
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {

                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                // SignIn settings
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            //services.AddAuthorization(config =>
            //{
            //    var defaultAuthBuilder = new AuthorizationPolicyBuilder();
            //    var defaultAuthPolicy = defaultAuthBuilder
            //    .RequireAuthenticatedUser()
            //    .RequireClaim(ClaimTypes.DateOfBirth)
            //    .Build();

            //    config.DefaultPolicy = defaultAuthPolicy;
            //});

            services.AddMailKit(config =>
            {
                config.UseMailKit(_config.GetSection("Email").Get<MailKitOptions>());
            });

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Home/Login";
            });

            // External services
            services.AddAuthentication()

                // Google authentications settings
                .AddGoogle(googleOptions =>
                {
                    IConfigurationSection googleAuthNSection = _config.GetSection("Authentication:Google");
                    googleOptions.ClientId = googleAuthNSection["ClientId"];
                    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                })

                // Facebook authentications settings
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = _config["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = _config["Authentication:Facebook:AppSecret"];
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            //who you are?
            app.UseAuthentication();

            //are you allowed?
            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
