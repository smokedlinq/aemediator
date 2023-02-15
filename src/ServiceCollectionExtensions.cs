using MediatR.Azure.EventGrid;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="EventGridMediator"/> and related services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddEventGridMediatR(this IServiceCollection services, Action<EventGridMediatorBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddScoped<EventGridMediator, DefaultEventGridMediator>();
        services.TryAddSingleton<EventGridDataDeserializer, DefaultEventGridDataDeserializer>();
        services.TryAddSingleton<EventGridDataDeserializerOptions>();
        services.TryAddSingleton<EventGridDataTypeResolver, DefaultEventGridDataTypeResolver>();

        var builder = new EventGridMediatorBuilder(services);
        configure?.Invoke(builder);

        return services;
    }
}
