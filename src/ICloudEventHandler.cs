using Azure.Messaging;

namespace MediatR.Azure.EventGrid;

public interface ICloudEventHandler<T> : INotificationHandler<EventNotification<CloudEvent, T>>
{
    Task HandleAsync(CloudEvent cloudEvent, T data, CancellationToken cancellationToken);

    Task INotificationHandler<EventNotification<CloudEvent, T>>.Handle(EventNotification<CloudEvent, T> notification, CancellationToken cancellationToken)
        => HandleAsync(notification.Event, notification.Data, cancellationToken);
}
