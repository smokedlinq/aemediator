using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

public abstract class EventDataTypeResolver
{
    public abstract Type? Resolve(EventDataType eventDataType);

    public virtual Type? Resolve(EventGridEvent eventGridEvent)
        => Resolve(new EventDataType(eventGridEvent.EventType, eventGridEvent.DataVersion));

    public virtual Type? Resolve(CloudEvent cloudEvent)
        => Resolve(new EventDataType(cloudEvent.Type, cloudEvent.DataSchema));

    internal Type? Resolve(object @event)
        => @event switch
            {
                EventGridEvent eventGridEvent => Resolve(eventGridEvent),
                CloudEvent cloudEvent => Resolve(cloudEvent),
                _ => throw new NotSupportedException($"The event type '{@event.GetType()}' is not supported.")
            };
}
