using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

public interface IEventGridEventHandler<T> : INotificationHandler<EventGridNotification<EventGridEvent, T>>
{
    Task HandleAsync(EventGridEvent eventGridEvent, T data, CancellationToken cancellationToken);

    Task INotificationHandler<EventGridNotification<EventGridEvent, T>>.Handle(EventGridNotification<EventGridEvent, T> notification, CancellationToken cancellationToken)
        => HandleAsync(notification.Event, notification.Data, cancellationToken);
}
