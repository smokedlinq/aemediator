using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

public abstract class EventGridDataTypeResolver
{
    public abstract Type? Resolve(EventGridDataType eventDataType);

    public virtual Type? Resolve(EventGridEvent eventGridEvent)
        => Resolve(new EventGridDataType(eventGridEvent.EventType, eventGridEvent.DataVersion));

    public virtual Type? Resolve(CloudEvent cloudEvent)
        => Resolve(new EventGridDataType(cloudEvent.Type, cloudEvent.DataSchema));

    internal Type? Resolve(object @event)
        => @event switch
            {
                EventGridEvent eventGridEvent => Resolve(eventGridEvent),
                CloudEvent cloudEvent => Resolve(cloudEvent),
                _ => throw new NotSupportedException($"The event type '{@event.GetType()}' is not supported.")
            };
}
