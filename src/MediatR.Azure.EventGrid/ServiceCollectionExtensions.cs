using MediatR.Azure.EventGrid;
using MediatR.Azure.EventGrid.Internals;
using MediatR.Azure.EventGrid.Serialization;
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
        _ = services ?? throw new ArgumentNullException(nameof(services));

        services.AddEventGridDataTypes();
        services.TryAddScoped<EventGridMediator, DefaultEventGridMediator>();

        var builder = new EventGridMediatorBuilder(services);
        configure?.Invoke(builder);

        return services;
    }
}
