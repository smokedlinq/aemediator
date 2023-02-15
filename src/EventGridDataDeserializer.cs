using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

public abstract class EventGridDataDeserializer
{
    public abstract object? Deserialize(Type type, BinaryData data);

    protected virtual object? Deserialize(EventGridEvent eventGridEvent, Type type, BinaryData data)
        => Deserialize(type, data);

    protected virtual object? Deserialize(CloudEvent cloudEvent, Type type, BinaryData data)
        => Deserialize(type, data);

    internal object? Deserialize(object @event, Type? type, BinaryData? data)
        => @event switch
        {
            _ when data is null => null,
            EventGridEvent eventGridEvent when type is not null => Deserialize(eventGridEvent, type, data),
            EventGridEvent eventGridEvent => eventGridEvent.TryGetSystemEventData(out var eventData) ? eventData : null,
            CloudEvent cloudEvent when type is not null => Deserialize(cloudEvent, type, data),
            CloudEvent cloudEvent => cloudEvent.TryGetSystemEventData(out var eventData) ? eventData : null,
            _ => throw new NotSupportedException($"The event type '{@event.GetType()}' is not supported.")
        };
}
