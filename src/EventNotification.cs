using System.ComponentModel;

namespace MediatR.Azure.EventGrid;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class EventNotification<TEvent, TData> : INotification
{
    public TEvent Event { get; }
    public TData Data { get; }

    internal EventNotification(TEvent @event, TData data)
    {
        Event = @event ?? throw new ArgumentNullException(nameof(@event));
        Data = data;
    }
}
