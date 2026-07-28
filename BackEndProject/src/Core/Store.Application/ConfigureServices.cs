using FluentValidation;
using System.Reflection;
using Edition.Application.Services;
using Edition.Application.Contracts;
using Edition.Application.Extensions;
using Edition.Application.Services.Orders;
using Edition.Application.Contracts.Orders;
using Edition.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using Edition.Application.Services.Localization;
using Edition.Application.Contracts.Localization;
using Edition.Application.Services.ContentPolicies;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Services;
using Edition.Application.Common.Utilities.Contracts;

namespace Edition.Application;

public static class ConfigureServices
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ContentPolicyQueryBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ContentPolicyResourceBehavior<,>));
        });

        services.AddScoped<IAccountingService, AccountingService>();
        services.AddScoped<IBankPaymentVerificationService, BankPaymentVerificationService>();
        services.AddScoped<IUserSessionService, UserSessionService>();
        services.AddScoped<IUserAuthCache, UserAuthCache>();
        services.AddScoped<ILocalFileService, LocalFileService>();

        services.AddSingleton<ContentPolicyCompiledFilterCache>();
        services.AddScoped<ContentPolicyRuleExpressionFactory>();
        services.AddScoped<ContentPolicyExpressionBuilder>();
        services.AddScoped<ContentPolicyRuleValidator>();
        services.AddScoped<ContentPolicyRecordAccessValidator>();
        services.AddScoped<IContentPolicyFilter, ContentPolicyFilter>();
        services.AddScoped<IContentPolicyCache, ContentPolicyCache>();
        services.AddScoped<IContentPolicyAccessChecker, ContentPolicyAccessChecker>();
        services.AddScoped<IContentPolicyResourceRequestResolver, ContentPolicyResourceRequestResolver>();
        services.AddScoped<IContentPolicyEntitySchemaExplorer, ContentPolicyEntitySchemaExplorer>();
        services.AddScoped<ContentPolicyScenarioEvaluator>();
        services.AddScoped<IContentPolicyPreviewService, ContentPolicyPreviewService>();

        services.AddSingleton<ILanguageRegistry, LanguageRegistry>();

        services.AddMapperServices(Assembly.GetExecutingAssembly());

        return services;
    }
}