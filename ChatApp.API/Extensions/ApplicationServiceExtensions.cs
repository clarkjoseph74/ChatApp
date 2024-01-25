using ChatApp.API.Data;
using ChatApp.API.Helpers;
using ChatApp.API.Interfaces;
using ChatApp.API.Middleware;
using ChatApp.API.Services;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        //DB Service
        services.AddDbContext<ApplicationDbContext>(
             options => options.UseSqlite(config.GetConnectionString("DefaultConnection"))
        );
        //Add My Contracts & Implementations

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<ExceptionMiddleware>();
        services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

        return services;
    }
}
