using HRIS.Application;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Infrastructure.Identity;
using HRIS.Infrastructure.Identity.Models;
using HRIS.Infrastructure.Interfaces.Repositories;
using HRIS.Infrastructure.Persistence;
using HRIS.Infrastructure.Shared;
using HRIS.Infrastructure.Shared.Services;
using HRIS.WebApi.Extensions;
using HRIS.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace HRIS.WebApi
{
    public class Startup
    {
        public IConfiguration _config { get; }
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowOrigin", options =>
            //    {
            //        options.AllowAnyOrigin();
            //        options.AllowAnyHeader();
            //        options.AllowAnyMethod();
            //    });
            //});

            services
               .AddCors(options =>
               {
                   options.AddPolicy("CorsPolicy", builder =>
                   {
                       builder.AllowAnyOrigin();
                       builder.AllowAnyMethod();
                       builder.AllowAnyHeader();
                   });
               });

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductsRepository, ProductsRepository>();
            services.AddTransient<IExpenditureRepository, ExpenditureRepository>();
            services.AddTransient<IStoreSetupRepository, StoreSetupRepository>();
            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<IBranchRepository, BranchRepository>();
            services.AddTransient<IDashBoardRepository, DashBoardRepository>();
            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(_config);
            services.AddPersistenceInfrastructure(_config);
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddScoped<IJwtService, JWTService>();
            services.AddSharedInfrastructure(_config);
            services.AddSwaggerExtension();
            services.AddControllers();
            services.AddApiVersioningExtension();
            services.AddHealthChecks();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            CreateRolesAndSuperUser(serviceProvider).Wait();
            app.UseHttpsRedirection();
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwaggerExtension();
            app.UseErrorHandlingMiddleware();
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
             });
        }

        private async Task CreateRolesAndSuperUser(IServiceProvider serviceProvider)
        {
            try
            {
                var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string[] roleNames = { "GlobalAdmin", "StoreAdmin", "BranchAdmin", "Customer", "Teller", "Account" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await RoleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        //create the roles and seed them to the database: Question 1
                        roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                //Here you could create a super user who will maintain the web app
                var poweruser = new ApplicationUser
                {

                    UserName = _config.GetSection("UserSettings")["UserName"],
                    Email = _config.GetSection("UserSettings")["UserEmail"],

                    EmailConfirmed = true
                };
                //Ensure you have these values in your appsettings.json file
                string userPWD = _config.GetSection("UserSettings")["UserPassword"];
                var _user = await UserManager.FindByEmailAsync(_config.GetSection("UserSettings")["UserEmail"]);

                if (_user == null)
                {
                    var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                    if (createPowerUser.Succeeded)
                    {
                        //here we tie the new user to the role
                        await UserManager.AddToRoleAsync(poweruser, "GlobalAdmin");

                    }
                }


            }
            catch (Exception ex)
            {
                // error occured
            }
            //initializing custom roles 

        }
    }
}
