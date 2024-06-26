using Library.Data;
using FluentValidation;
using Library.Model.Models;
using Library.Service.Interfaces;
using Library.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Library.Model.Interfaces;
using Library.Data.Repositories;
using Library.Data.Configurations.Variables;
using Library.Service.Services.Logger;

namespace Library.Extensions;

public static class DependencyInjection
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>(); // includes all repositories
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>(); // includes all services
        services.AddScoped<IValidationService, ValidationService>(); // except validation, since it's only used inside other services
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void ConfigureMailjet(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<MailjetSettings>(configuration.GetSection("MailjetSettings"));

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
