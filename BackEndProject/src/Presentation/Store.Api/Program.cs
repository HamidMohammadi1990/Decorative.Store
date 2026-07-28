using Serilog;
using Edition.Application;
using Edition.Application.Contracts.Localization;
using Store.WebFramework.Extensions;
using Store.WebFramework.Swagger;
using Store.Api.Middlewares;
using Store.Api.Extensions;
using Store.Infrastructure.LogProviders;
using Store.Infrastructure.Configurations;
using Store.Infrastructure;
using Store.Infrastructure.Persistence;
using Store.Common.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog();

builder.AddAutofactServiceProviderAndInterceptors();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEditionSecurity(builder.Configuration);
builder.Services.AddEditionLocalization(builder.Configuration);
builder.Services.AddSingleton(
    builder.Configuration.GetSection(nameof(ForwardedHeadersSettings)).Get<ForwardedHeadersSettings>()
    ?? new ForwardedHeadersSettings());

builder.Services.AddCustomCors(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEmailTokenProviderConfiguration(builder.Configuration);
builder.Services.AddPhoneNumberTokenProviderConfiguration(builder.Configuration);
builder.Services.AddContentPolicyCacheConfiguration(builder.Configuration);
builder.Services.AddLanguageCacheConfiguration(builder.Configuration);
builder.Services.RegisterApplicationServices()
                .RegisterInfrastructureServices(builder.Configuration)
                .RegisterPersistenceServices(builder.Configuration, builder.Environment.IsDevelopment());

var siteSettings = builder.Configuration
    .GetSection(nameof(SiteSettings)).Get<SiteSettings>()
    ?? throw new NullReferenceException("siteSettings is null ...");

builder.Services.AddSwagger();
builder.Services.AddSingleton(siteSettings!);
builder.Services.AddMinimalMvc();
builder.Services.AddJwtAuthentication(siteSettings!.JwtSettings, builder.Environment);
builder.Services.AddCustomApiVersioning();
builder.Services.AddCustomResponseCompression();

var serilogConfiguration = builder.Configuration.GetSection("Serilog:Configuration").Get<SeilogConfiguration>();
serilogConfiguration!.UseSerilog(builder.Configuration);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var languageBootstrap = scope.ServiceProvider.GetRequiredService<ILanguageBootstrapService>();
    await languageBootstrap.EnsureLanguagesReadyAsync();
}

await app.ConfigureEditionLocalizationFromRegistryAsync();

app.UseEditionLocalization();
app.UseMiddleware<CurrentLanguageMiddleware>();
app.UseMiddleware<BlockTokenControlMiddleware>();

if (app.Environment.IsDevelopment())
{
    //using var scope = app.Services.CreateScope();
    //var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
    //var permissions = PermissionModule.GetPermissions();
    //await seedService.SeedDataAsync(permissions);
}

if (!builder.Environment.IsDevelopment())
    app.UseHsts();

app.UserCustomeForwardedHeaders();

app.UseCustomExceptionHandler();
app.UseCors("CustomCors");
app.UseHttpsRedirection();
app.UseSwaggerAndUI();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

//app.CloseSerilogWhenApplicationStopping();

await app.CustomRunAsync();