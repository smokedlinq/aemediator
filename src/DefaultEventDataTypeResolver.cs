using System.Collections.Concurrent;

namespace MediatR.Azure.EventGrid;

internal sealed class DefaultEventDataTypeResolver : EventDataTypeResolver
{
    private readonly ConcurrentDictionary<EventDataType, Type?> _cache = new();

    public DefaultEventDataTypeResolver(IEnumerable<EventDataTypeRegistration> registrations)
    {
        foreach (var registration in registrations)
            _cache.TryAdd(registration.DataType, registration.Type);
    }

    public override Type? Resolve(EventDataType eventDataType)
        => _cache.GetOrAdd(eventDataType, GetType);

    private static Type? GetType(EventDataType type)
        => Type.GetType(type.Type) ?? FindTypeInAssemblies(type.Type);

    private static Type? FindTypeInAssemblies(string type)
        => AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetType(type)).FirstOrDefault();
}
