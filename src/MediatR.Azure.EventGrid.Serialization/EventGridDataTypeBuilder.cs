using System.Reflection;
using System.Text.Json;
using MediatR.Azure.EventGrid.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Azure.EventGrid;

/// <summary>
/// Represents a builder for configuring the <see cref="EventGridDataTypeBuilder"/> and related services.
/// </summary>
public sealed class EventGridDataTypeBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventGridDataTypeBuilder"/> class.
    /// </summary>
    public EventGridDataTypeBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> where the <see cref="EventGridDataTypeBuilder"/> and related services are configured.
    /// </summary>
    public IServiceCollection Services { get; }
}

/// <summary>
/// Provides extension methods for the <see cref="EventGridDataTypeBuilder"/> class.
/// </summary>
public static class EventGridMediatorBuilderExtensions
{
    /// <summary>
    /// Adds a data type to the <see cref="EventGridDataTypeBuilder"/>.
    /// </summary>
    public static EventGridDataTypeBuilder Add<T>(this EventGridDataTypeBuilder builder, string? dataType = null, string? dataVersion = null)
        => builder.Add(dataType ?? typeof(T).Name, dataVersion, typeof(T));

    /// <summary>
    /// Adds a data type to the <see cref="EventGridDataTypeBuilder"/>.
    /// </summary>
    public static EventGridDataTypeBuilder Add(this EventGridDataTypeBuilder builder, string dataType, string? dataVersion, Type type)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));
        _ = dataType ?? throw new ArgumentNullException(nameof(dataType));
        _ = type ?? throw new ArgumentNullException(nameof(type));

        var eventDataType = new EventGridDataType(dataType, dataVersion);
        builder.Services.AddSingleton(new EventGridDataTypeRegistration(eventDataType, type));

        return builder;
    }

    /// <summary>
    /// Adds all types with the <see cref="EventGridDataTypeAttribute" /> from the specified assembly to the <see cref="EventGridDataTypeBuilder"/>.
    /// </summary>
    public static EventGridDataTypeBuilder RegisterFromAssembly(this EventGridDataTypeBuilder builder, Assembly assembly)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        var types = assembly.GetExportedTypes()
            .SelectMany(type => type.GetCustomAttributes<EventGridDataTypeAttribute>()
                .Select(attribute => (attribute, type)));

        foreach (var (attribute, type) in types)
        {
            builder.Add(attribute.Type, attribute.Version, type);
        }

        return builder;
    }
}
