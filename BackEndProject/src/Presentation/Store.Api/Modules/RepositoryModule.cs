using Autofac;
using Castle.DynamicProxy;
using Autofac.Extras.DynamicProxy;
using Store.Infrastructure.Interceptors;
using Store.Infrastructure.Persistence;

namespace Store.Api.Modules;

/// <summary>
/// Set Cache Interceptor For Repositories
/// </summary>
public class RepositoryModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CacheInterceptor>()
               .As<IAsyncInterceptor>()
               .InstancePerLifetimeScope();

        builder.Register(c =>
            new AsyncDeterminationInterceptor(c.Resolve<IAsyncInterceptor>()))
            .As<IInterceptor>()
            .InstancePerLifetimeScope();

        var infrastructurePersistence = typeof(UnitOfWork).Assembly;

        builder.RegisterAssemblyTypes(infrastructurePersistence)
               .Where(x => x.Name.EndsWith("Repository"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope()
               .EnableInterfaceInterceptors()
               .InterceptedBy(typeof(IInterceptor));
    }
}