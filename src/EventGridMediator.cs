using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

public abstract class EventGridMediator
{
    public abstract Task PublishAsync(EventGridEvent eventGridEvent, CancellationToken cancellationToken = default);

    public abstract Task PublishAsync(CloudEvent cloudEvent, CancellationToken cancellationToken = default);
}
