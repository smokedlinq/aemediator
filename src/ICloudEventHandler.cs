using Azure.Messaging;

namespace MediatR.Azure.EventGrid;

public interface ICloudEventHandler<T> : INotificationHandler<EventGridNotification<CloudEvent, T>>
{
    Task HandleAsync(CloudEvent cloudEvent, T data, CancellationToken cancellationToken);

    Task INotificationHandler<EventGridNotification<CloudEvent, T>>.Handle(EventGridNotification<CloudEvent, T> notification, CancellationToken cancellationToken)
        => HandleAsync(notification.Event, notification.Data, cancellationToken);
}
