namespace MediatR.Azure.EventGrid.Serialization.Internals;

internal sealed class DefaultEventGridDataTypeResolver : EventGridDataTypeResolver
{
    private readonly Dictionary<EventGridDataType, Type?> _eventGridDataTypes = new();
    private readonly Dictionary<Type, EventGridDataType> _types = new();

    public DefaultEventGridDataTypeResolver(IEnumerable<EventGridDataTypeRegistration> registrations)
    {
        foreach (var registration in registrations)
        {
            _eventGridDataTypes.Add(registration.DataType, registration.Type);
            _types.Add(registration.Type, registration.DataType);
        }
    }

    public override Type? Resolve(EventGridDataType eventDataType)
        => _eventGridDataTypes.TryGetValue(eventDataType, out var type) ? type : null;

    public override EventGridDataType? Resolve(Type type)
        => _types.TryGetValue(type, out var eventGridDataType)
            ? eventGridDataType
            : null;
}
