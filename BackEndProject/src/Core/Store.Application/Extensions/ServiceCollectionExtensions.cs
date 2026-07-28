using System.Reflection;
using Edition.Application.Contracts.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Edition.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMapperServices(this IServiceCollection services, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assembly);

        var mapperInterfaces = assembly
            .GetTypes()
            .Where(t => t is { IsInterface: true, IsPublic: true }
                        && t != typeof(IMapper)
                        && typeof(IMapper).IsAssignableFrom(t))
            .OrderBy(t => t.FullName, StringComparer.Ordinal)
            .ToArray();

        if (mapperInterfaces.Length == 0)
            throw new InvalidOperationException($"No mapper interfaces inheriting {nameof(IMapper)} were found in assembly '{assembly.GetName().Name}'.");

        var concreteTypes = assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, IsPublic: true })
            .ToArray();

        foreach (var mapperInterface in mapperInterfaces)
        {
            var implementations = concreteTypes
                .Where(mapperInterface.IsAssignableFrom)
                .ToArray();

            switch (implementations.Length)
            {
                case 0:
                    throw new InvalidOperationException(
                        $"Mapper implementation not found for '{mapperInterface.FullName}'.");
                case > 1:
                    throw new InvalidOperationException(
                        $"Multiple mapper implementations found for '{mapperInterface.FullName}': " +
                        string.Join(", ", implementations.Select(x => x.FullName)));
            }

            services.TryAddSingleton(mapperInterface, implementations[0]);
        }

        return services;
    }
}