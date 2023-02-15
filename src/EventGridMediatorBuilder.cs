using System.Reflection;
using System.Text.Json;
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
    }

    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> where the <see cref="EventGridMediator"/> and related services are configured.
    /// </summary>
    public IServiceCollection Services { get; }
}

/// <summary>
/// Provides extension methods for the <see cref="EventGridMediatorBuilder"/> class.
/// </summary>
public static class EventGridMediatorBuilderExtensions
{
    /// <summary>
    /// Adds a data type to the <see cref="EventGridMediatorBuilder"/>.
    /// </summary>
    public static EventGridMediatorBuilder AddDataType<T>(this EventGridMediatorBuilder builder, string? dataType = null, string? dataVersion = null)
        => builder.AddDataType(dataType ?? typeof(T).Name, dataVersion, typeof(T));

    /// <summary>
    /// Adds a data type to the <see cref="EventGridMediatorBuilder"/>.
    /// </summary>
    public static EventGridMediatorBuilder AddDataType(this EventGridMediatorBuilder builder, string dataType, string? dataVersion, Type type)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(dataType);
        ArgumentNullException.ThrowIfNull(type);

        var eventDataType = new EventGridDataType(dataType, dataVersion);
        builder.Services.AddSingleton(new EventGridDataTypeRegistration(eventDataType, type));

        return builder;
    }

    /// <summary>
    /// Adds all types with the <see cref="EventGridDataTypeAttribute" /> from the specified assembly to the <see cref="EventGridMediatorBuilder"/>.
    /// </summary>
    public static EventGridMediatorBuilder RegisterDataTypesFromAssembly(this EventGridMediatorBuilder builder, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        var types = assembly.GetExportedTypes()
            .SelectMany(type => type.GetCustomAttributes<EventGridDataTypeAttribute>()
                .Select(attribute => (attribute, type)));

        foreach (var (attribute, type) in types)
        {
            builder.AddDataType(attribute.Type, attribute.Version, type);
        }

        return builder;
    }

    /// <summary>
    /// Configures the <see cref="JsonSerializerOptions"/> to use.
    /// </summary>
    public static EventGridMediatorBuilder ConfigureJsonSerializerOptions(this EventGridMediatorBuilder builder, Action<JsonSerializerOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        return ConfigureJsonSerializerOptions(builder, (_, options) => configure(options));
    }

    /// <summary>
    /// Configures the <see cref="JsonSerializerOptions"/> to use.
    /// </summary>
    public static EventGridMediatorBuilder ConfigureJsonSerializerOptions(this EventGridMediatorBuilder builder, Action<IServiceProvider, JsonSerializerOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        builder.Services.AddSingleton(serviceProvider =>
        {
            var options = new EventGridDataDeserializerOptions();
            configure(serviceProvider, options.JsonSerializerOptions);
            return options;
        });

        return builder;
    }

    /// <summary>
    /// Configures the <see cref="EventGridDataDeserializer"/> to use.
    /// </summary>
    public static EventGridMediatorBuilder UseDataDeserializer<T>(this EventGridMediatorBuilder builder, Func<IServiceProvider, EventGridDataDeserializer>? factory = null)
        where T : EventGridDataDeserializer
    {
        ArgumentNullException.ThrowIfNull(builder);

        _ = factory is null
            ? builder.Services.AddSingleton<EventGridDataDeserializer, T>()
            : builder.Services.AddSingleton<EventGridDataDeserializer>(factory);

        return builder;
    }

    /// <summary>
    /// Configures the <see cref="EventGridDataTypeResolver"/> to use.
    /// </summary>
    public static EventGridMediatorBuilder UseDataTypeResolver<T>(this EventGridMediatorBuilder builder, Func<IServiceProvider, EventGridDataTypeResolver>? factory = null)
        where T : EventGridDataTypeResolver
    {
        ArgumentNullException.ThrowIfNull(builder);

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
        ArgumentNullException.ThrowIfNull(builder);

        _ = factory is null
            ? builder.Services.AddSingleton<EventGridMediator, T>()
            : builder.Services.AddSingleton<EventGridMediator>(factory);

        return builder;
    }
}
