using Azure.Messaging;

namespace MediatR.Azure.EventGrid;

/// <summary>
/// Represents a handler for a <see cref="CloudEvent"/> event.
/// </summary>
public interface ICloudEventHandler<T> : INotificationHandler<EventGridNotification<CloudEvent, T>>
{
    /// <summary>
    /// Handles the <see cref="CloudEvent"/>.
    /// </summary>
    Task HandleAsync(CloudEvent cloudEvent, T data, CancellationToken cancellationToken);

    Task INotificationHandler<EventGridNotification<CloudEvent, T>>.Handle(EventGridNotification<CloudEvent, T> notification, CancellationToken cancellationToken)
        => HandleAsync(notification.Event, notification.Data, cancellationToken);
}
