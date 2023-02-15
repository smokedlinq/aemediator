using System.Collections.Concurrent;
using MediatR.Azure.EventGrid.Serialization;

namespace MediatR.Azure.EventGrid.Internals;

internal sealed class DefaultEventGridDataTypeResolver : EventGridDataTypeResolver
{
    private readonly ConcurrentDictionary<EventGridDataType, Type?> _cache = new();

    public DefaultEventGridDataTypeResolver(IEnumerable<EventGridDataTypeRegistration> registrations)
    {
        foreach (var registration in registrations)
            _cache.TryAdd(registration.DataType, registration.Type);
    }

    public override Type? Resolve(EventGridDataType eventDataType)
        => _cache.GetOrAdd(eventDataType, GetType);

    private static Type? GetType(EventGridDataType type)
        => Type.GetType(type.Type) ?? FindTypeInAssemblies(type.Type);

    private static Type? FindTypeInAssemblies(string type)
        => AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetType(type)).FirstOrDefault();
}
