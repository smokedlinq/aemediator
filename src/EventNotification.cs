using System.ComponentModel;

namespace MediatR.Azure.EventGrid;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class EventNotification<TEvent, TData> : INotification
{
    public TEvent Event { get; }
    public TData Data { get; }

    public EventNotification(TEvent @event, TData data)
    {
        Event = @event ?? throw new ArgumentNullException(nameof(@event));
        Data = data;
    }
}
