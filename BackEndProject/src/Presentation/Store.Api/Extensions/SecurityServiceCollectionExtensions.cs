using System.Net;
using System.Text;
using System.Security.Claims;
using Store.Common.Exceptions;
using Microsoft.Extensions.Options;
using Edition.Application.Contracts;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Store.Common.Models;
using Store.Common.Security;
using Store.Common.Enums;
using Store.Common.Localization;

namespace Store.Api.Extensions;

public static class SecurityServiceCollectionExtensions
{
    public static IServiceCollection AddEditionSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>()
            ?? throw new InvalidOperationException("SecuritySettings section is missing.");

        SecurityKeyRegistry.Initialize(securitySettings);
        services.AddSingleton(securitySettings);
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        JwtSettings jwtSettings,
        IHostEnvironment environment)
    {
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
        var encryptionKey = Encoding.UTF8.GetBytes(jwtSettings.EncryptKey);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
        };

        services.AddSingleton(tokenValidationParameters);
        services.AddSingleton(sp => Options.Create(sp.GetRequiredService<TokenValidationParameters>()));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.MapInboundClaims = true;
            options.RequireHttpsMetadata = !environment.IsDevelopment();
            options.SaveToken = true;
            options.TokenValidationParameters = tokenValidationParameters;
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception != null)
                        throw new AppException(
                            OperationStatusCode.UnAuthorized,
                            GetLocalizedMessage(context.HttpContext, MessageKeys.AuthenticationFailed),
                            HttpStatusCode.Unauthorized,
                            context.Exception,
                            null);

                    return Task.CompletedTask;
                },
                OnTokenValidated = async context =>
                {
                    var claimsIdentity = context.Principal!.Identity as ClaimsIdentity;
                    if (claimsIdentity!.Claims?.Any() != true)
                    {
                        context.Fail(GetLocalizedMessage(context.HttpContext, MessageKeys.InvalidToken));
                        return;
                    }

                    var validator = context.HttpContext.RequestServices.GetRequiredService<IAuthContextValidator>();
                    if (!await validator.ValidateAsync(context.Principal!, context.HttpContext.RequestAborted))
                        context.Fail(GetLocalizedMessage(context.HttpContext, MessageKeys.InvalidToken));
                },
                OnChallenge = context =>
                {
                    if (context.AuthenticateFailure != null)
                        throw new AppException(
                            OperationStatusCode.UnAuthorized,
                            GetLocalizedMessage(context.HttpContext, MessageKeys.AuthenticateFailure),
                            HttpStatusCode.Unauthorized,
                            context.AuthenticateFailure,
                            null);

                    throw new AppException(
                        OperationStatusCode.UnAuthorized,
                        GetLocalizedMessage(context.HttpContext, MessageKeys.UnauthorizedResourceAccess),
                        HttpStatusCode.Unauthorized);
                }
            };
        });

        return services;
    }

    private static string GetLocalizedMessage(HttpContext httpContext, string messageKey)
        => httpContext.RequestServices
            .GetRequiredService<IResourceManager>()
            .GetString(messageKey);
}