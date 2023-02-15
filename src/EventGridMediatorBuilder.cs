using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Azure.EventGrid;

public sealed class EventGridMediatorBuilder
{
    public EventGridMediatorBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public IServiceCollection Services { get; }
}

public static class EventGridMediatorBuilderExtensions
{
    public static EventGridMediatorBuilder AddDataType<T>(this EventGridMediatorBuilder builder, string? dataType = null, string? dataVersion = null)
        => builder.AddDataType(dataType ?? typeof(T).Name, dataVersion, typeof(T));

    public static EventGridMediatorBuilder AddDataType(this EventGridMediatorBuilder builder, string dataType, string? dataVersion, Type type)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(dataType);
        ArgumentNullException.ThrowIfNull(type);

        var eventDataType = new EventGridDataType(dataType, dataVersion);
        builder.Services.AddSingleton(new EventGridDataTypeRegistration(eventDataType, type));

        return builder;
    }

    public static EventGridMediatorBuilder ConfigureDeserialize(this EventGridMediatorBuilder builder, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(options);

        builder.Services.AddSingleton<EventGridDataDeserializer>(_ => new DefaultEventGridDataDeserializer(options));

        return builder;
    }

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
}
