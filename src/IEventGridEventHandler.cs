using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

public interface IEventGridEventHandler<T> : INotificationHandler<EventNotification<EventGridEvent, T>>
{
    Task HandleAsync(EventGridEvent eventGridEvent, T data, CancellationToken cancellationToken);

    Task INotificationHandler<EventNotification<EventGridEvent, T>>.Handle(EventNotification<EventGridEvent, T> notification, CancellationToken cancellationToken)
        => HandleAsync(notification.Event, notification.Data, cancellationToken);
}
