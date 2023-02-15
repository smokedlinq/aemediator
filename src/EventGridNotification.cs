using System.ComponentModel;
using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class EventGridNotification<TEvent, TData> : INotification
{
    public TEvent Event { get; }
    public TData Data { get; }

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
