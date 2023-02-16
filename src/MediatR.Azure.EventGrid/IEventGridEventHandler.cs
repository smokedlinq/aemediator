using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

/// <summary>
/// Represents a handler for an <see cref="EventGridEvent"/> event.
/// </summary>
public interface IEventGridEventHandler<T> : INotificationHandler<EventGridNotification<EventGridEvent, T>>
{
    /// <summary>
    /// Handles the <see cref="EventGridEvent"/>.
    /// </summary>
    Task HandleAsync(EventGridEvent eventGridEvent, T data, CancellationToken cancellationToken);

    Task INotificationHandler<EventGridNotification<EventGridEvent, T>>.Handle(EventGridNotification<EventGridEvent, T> notification, CancellationToken cancellationToken)
        => HandleAsync(notification.Event, notification.Data, cancellationToken);
}
