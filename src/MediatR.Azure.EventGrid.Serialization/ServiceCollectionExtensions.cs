using MediatR.Azure.EventGrid.Serialization.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MediatR.Azure.EventGrid.Serialization;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="EventGridDataType"/> and related services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddEventGridDataTypes(this IServiceCollection services, Action<EventGridDataTypeBuilder>? configure = null)
    {
        _ = services ?? throw new ArgumentNullException(nameof(services));

        services.TryAddSingleton<CloudEventFactory, DefaultCloudEventFactory>();
        services.TryAddSingleton<EventGridEventFactory, DefaultEventGridEventFactory>();
        services.TryAddSingleton<EventGridDataSerializerOptions>();
        services.TryAddSingleton<EventGridDataSerializer, DefaultEventGridDataSerializer>();
        services.TryAddSingleton<EventGridDataTypeResolver, DefaultEventGridDataTypeResolver>();

        var builder = new EventGridDataTypeBuilder(services);
        configure?.Invoke(builder);

        return services;
    }
}
