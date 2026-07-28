using Autofac;
using Edition.Api.Modules;
using Autofac.Extensions.DependencyInjection;

namespace Store.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddAutofactServiceProviderAndInterceptors(this WebApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        applicationBuilder.Host.ConfigureContainer<ContainerBuilder>
                     (builder => builder.RegisterModule(new RepositoryModule()));
    }
}