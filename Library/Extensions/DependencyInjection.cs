﻿using Library.Data;
using FluentValidation;
using Library.Model.Models;
using Library.Service.Interfaces;
using Library.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Library.Extensions;

public static class DependencyInjection
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        // unit of work not yet implemented
        //services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void ConfigureServices(this IServiceCollection services) // here we'll add all our services
    {
        services.AddScoped<IAuthService, AuthService>();
    }

    public static void ConfigureSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void ConfigureValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<Employee, IdentityRole>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireUppercase = true;
            o.Password.RequireNonAlphanumeric = true;
            o.Password.RequiredLength = 8;
            o.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }
}
