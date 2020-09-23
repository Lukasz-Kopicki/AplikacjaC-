
using Application.Interfaces;
using Application.Services;
using DataAccessLogic.Entities;
using DataAccessLogic.Interfaces;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace IoC
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesRepository, SQLCategoryRepository>();
            services.AddScoped<IUserManager, UserManagerService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            

        }
    }
}
