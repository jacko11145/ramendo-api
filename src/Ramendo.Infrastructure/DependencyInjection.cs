using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ramendo.Application.Common;
using Ramendo.Domain.Aggregates.InvitationCodes;
using Ramendo.Domain.Aggregates.Reviews;
using Ramendo.Domain.Aggregates.Shops;
using Ramendo.Domain.Aggregates.Submissions;
using Ramendo.Domain.Aggregates.Users;
using Ramendo.Domain.Services;
using Ramendo.Infrastructure.Persistence;
using Ramendo.Infrastructure.Persistence.Repositories;
using Ramendo.Infrastructure.Services;

namespace Ramendo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<RamendoDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"))
                   .UseSnakeCaseNamingConvention());

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRamenShopRepository, RamenShopRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IShopSubmissionRepository, ShopSubmissionRepository>();
        services.AddScoped<IInvitationCodeRepository, InvitationCodeRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<ISystemSettingsRepository, SystemSettingsRepository>();

        // Domain Services
        services.AddScoped<IRankingService, RankingService>();
        services.AddScoped<IPermissionService, PermissionService>();

        // Application Services (interfaces)
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IGoogleTokenValidator, GoogleTokenValidatorService>();

        return services;
    }
}
