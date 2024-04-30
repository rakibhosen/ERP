using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ERP.SERVICE;
using ERP.SERVICE.IRepositories.Auth;
using ERP.SERVICE.IRepositories.HRM;
using ERP.SERVICE.IRepositories.Menu;
using ERP.SERVICE.Middleware;
using ERP.SERVICE.Repositories.Auth;
using ERP.SERVICE.Repositories.HRM;
using ERP.SERVICE.Repositories.Menu;
using ERP.UTILITY;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.WEB
{


    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("data");

            // Register DataAccess
            services.AddSingleton<DataAccess>(provider => new DataAccess(connectionString));


            // Register repositories
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserPagePermissionRepository, UserPagePermissionRepository>(); // Register IUserPagePermissionRepository
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<ICodeBookReposiroty, CodeBookRepository>();

            services.AddResponseCaching();



        }

        public static void UsePagePermissionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<PagePermissionMiddleware>();
        }
    }

}
