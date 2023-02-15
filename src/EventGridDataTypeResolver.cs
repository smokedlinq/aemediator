using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

/// <summary>
/// Represents a resolver for event data types.
/// </summary>
public abstract class EventGridDataTypeResolver
{
    /// <summary>
    /// Resolves the event data type to a .NET type.
    /// </summary>
    public abstract Type? Resolve(EventGridDataType eventDataType);

    /// <summary>
    /// Resolves the <see cref="EventGridEvent" /> to a .NET type.
    /// </summary>
    public virtual Type? Resolve(EventGridEvent eventGridEvent)
        => Resolve(new EventGridDataType(eventGridEvent.EventType, eventGridEvent.DataVersion));

    /// <summary>
    /// Resolves the <see cref="CloudEvent" /> to a .NET type.
    /// </summary>
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
