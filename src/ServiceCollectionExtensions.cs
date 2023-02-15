using MediatR.Azure.EventGrid;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventGridMediatR(this IServiceCollection services, Action<EventGridMediatorBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddScoped<EventGridMediator, DefaultEventGridMediator>();
        services.TryAddSingleton<EventDataDeserializer, DefaultEventDataDeserializer>();
        services.TryAddSingleton<EventDataTypeResolver, DefaultEventDataTypeResolver>();

        var builder = new EventGridMediatorBuilder(services);
        configure?.Invoke(builder);

        return services;
    }
}
