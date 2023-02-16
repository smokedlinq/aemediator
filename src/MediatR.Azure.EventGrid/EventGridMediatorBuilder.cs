using System.Reflection;
using System.Text.Json;
using MediatR.Azure.EventGrid.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Azure.EventGrid;

/// <summary>
/// Represents a builder for configuring the <see cref="EventGridMediator"/> and related services.
/// </summary>
public sealed class EventGridMediatorBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventGridMediatorBuilder"/> class.
    /// </summary>
    public EventGridMediatorBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
        DataTypes = new EventGridDataTypeBuilder(services);
    }

    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> where the <see cref="EventGridMediator"/> and related services are configured.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets the <see cref="EventGridDataTypeBuilder"/> where the <see cref="EventGridDataType"/> and related services are configured.
    /// </summary>
    public EventGridDataTypeBuilder DataTypes { get; }
}

/// <summary>
/// Provides extension methods for the <see cref="EventGridMediatorBuilder"/> class.
/// </summary>
public static class EventGridMediatorBuilderExtensions
{
    /// <summary>
    /// Configures the <see cref="JsonSerializerOptions"/> to use.
    /// </summary>
    public static EventGridMediatorBuilder ConfigureJsonSerializerOptions(this EventGridMediatorBuilder builder, Action<JsonSerializerOptions> configure)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));
        _ = configure ?? throw new ArgumentNullException(nameof(configure));

        return ConfigureJsonSerializerOptions(builder, (_, options) => configure(options));
    }

    /// <summary>
    /// Configures the <see cref="JsonSerializerOptions"/> to use.
    /// </summary>
    public static EventGridMediatorBuilder ConfigureJsonSerializerOptions(this EventGridMediatorBuilder builder, Action<IServiceProvider, JsonSerializerOptions> configure)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));
        _ = configure ?? throw new ArgumentNullException(nameof(configure));

        builder.Services.AddSingleton(serviceProvider =>
        {
            var options = new EventGridDataSerializerOptions();
            configure(serviceProvider, options.JsonSerializerOptions);
            return options;
        });

        return builder;
    }

    /// <summary>
    /// Configures the <see cref="EventGridDataSerializer"/> to use.
    /// </summary>
    public static EventGridMediatorBuilder UseDataDeserializer<T>(this EventGridMediatorBuilder builder, Func<IServiceProvider, EventGridDataSerializer>? factory = null)
        where T : EventGridDataSerializer
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        _ = factory is null
            ? builder.Services.AddSingleton<EventGridDataSerializer, T>()
            : builder.Services.AddSingleton<EventGridDataSerializer>(factory);

        return builder;
    }

    /// <summary>
    /// Configures the <see cref="EventGridDataTypeResolver"/> to use.
    /// </summary>
    public static EventGridMediatorBuilder UseDataTypeResolver<T>(this EventGridMediatorBuilder builder, Func<IServiceProvider, EventGridDataTypeResolver>? factory = null)
        where T : EventGridDataTypeResolver
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        _ = factory is null
            ? builder.Services.AddSingleton<EventGridDataTypeResolver, T>()
            : builder.Services.AddSingleton<EventGridDataTypeResolver>(factory);

        return builder;
    }

    /// <summary>
    /// Configures the <see cref="EventGridMediator"/> to use.
    /// </summary>
    public static EventGridMediatorBuilder UseMediator<T>(this EventGridMediatorBuilder builder, Func<IServiceProvider, EventGridMediator>? factory = null)
        where T : EventGridMediator
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        _ = factory is null
            ? builder.Services.AddSingleton<EventGridMediator, T>()
            : builder.Services.AddSingleton<EventGridMediator>(factory);

        return builder;
    }
}
