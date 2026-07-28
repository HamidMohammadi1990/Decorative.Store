using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Edition.Application.Contracts.Orders;
using Microsoft.Extensions.DependencyInjection;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.ContentPolicies;
using Store.Infrastructure.Persistence.Services;
using Store.Infrastructure.Persistence.Localization;
using Store.Infrastructure.Persistence.ContentPolicies;
using Store.Infrastructure.Persistence.SeedData;
using Store.Infrastructure.Persistence.Contracts;
using Store.Infrastructure.Persistence.Repositories;
using Store.Domain.ContentPolicies;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection RegisterPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration,
        bool includeDetailedSaveErrors = false)
    {
        services.AddSingleton<ISaveChangesExceptionReporting>(
            new SaveChangesExceptionReporting(includeDetailedSaveErrors));

        services
            .AddDbContext<EditionDbContext>(
            options => options.UseSqlServer(configuration
            .GetConnectionString("EditionDbContext")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IContentPolicyEntityAccessQuery, ContentPolicyEntityAccessQuery>();
        services.AddScoped<IContentPolicyEntityPreviewQuery, ContentPolicyEntityPreviewQuery>();

        services.AddSingleton<IContentEntityTypeRegistry, DbContextContentEntityTypeRegistry>();

        services.AddScoped<IContentPolicyMetadataRepository, ContentPolicyMetadataRepository>();

        // Services
        services.AddScoped<ISeedService, SeedService>();
        services.AddScoped<ILanguageBootstrapService, LanguageBootstrapService>();
        services.AddScoped<IOrderTrackingCodeGenerator, OrderTrackingCodeGenerator>();

        return services;
    }
}