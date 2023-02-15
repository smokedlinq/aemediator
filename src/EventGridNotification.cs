using System.ComponentModel;
using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

/// <summary>
/// Represents a notification that is sent to the <see cref="IMediator"/> when an event is received from Event Grid.
/// </summary>
/// <remarks>
/// Note: This type should not be used directly.
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class EventGridNotification<TEvent, TData> : INotification
{
    /// <summary>
    /// Gets the EventGrid event, either a <see cref="EventGridEvent"/> or <see cref="CloudEvent"/>.
    /// </summary>
    public TEvent Event { get; }

    /// <summary>
    /// Gets the deserialized data of the event.
    /// </summary>
    public TData Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventGridNotification{TEvent, TData}"/> class.
    /// </summary>
    public EventGridNotification(TEvent @event, TData data)
    {
        Event = @event switch
        {
            _ when @event is null => throw new ArgumentNullException(nameof(@event)),
            EventGridEvent => @event,
            CloudEvent => @event,
            _ => throw new ArgumentException($"The type of the event must be either '{typeof(EventGridEvent)}' or '{typeof(CloudEvent)}'.", nameof(@event))
        };
        Data = data;
    }
}
