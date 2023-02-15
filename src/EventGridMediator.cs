using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

public abstract class EventGridMediator
{
    public abstract Task PublishAsync(EventGridEvent eventGridEvent, CancellationToken cancellationToken = default);
    public abstract Task PublishAsync(CloudEvent cloudEvent, CancellationToken cancellationToken = default);

    public virtual async Task PublishAsync(IEnumerable<EventGridEvent> eventGridEvents, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventGridEvents);

        foreach (var @event in eventGridEvents)
        {
            await PublishAsync(@event, cancellationToken).ConfigureAwait(false);
        }
    }

    public virtual async Task PublishAsync(IEnumerable<CloudEvent> cloudEvents, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(cloudEvents);

        foreach (var @event in cloudEvents)
        {
            await PublishAsync(@event, cancellationToken).ConfigureAwait(false);
        }
    }
}
