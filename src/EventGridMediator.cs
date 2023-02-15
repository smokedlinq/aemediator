using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

/// <summary>
/// Represents a mediator for publishing events form Event Grid.
/// </summary>
public abstract class EventGridMediator
{
    /// <summary>
    /// Publishes the specified <see cref="EventGridEvent"/> event.
    /// </summary>
    public abstract Task PublishAsync(EventGridEvent eventGridEvent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes the specified <see cref="CloudEvent"/> event.
    /// </summary>
    public abstract Task PublishAsync(CloudEvent cloudEvent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes the specified <see cref="EventGridEvent"/> events.
    /// </summary>
    public virtual async Task PublishAsync(IEnumerable<EventGridEvent> eventGridEvents, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventGridEvents);

        foreach (var @event in eventGridEvents)
        {
            await PublishAsync(@event, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Publishes the specified <see cref="CloudEvent"/> events.
    /// </summary>
    public virtual async Task PublishAsync(IEnumerable<CloudEvent> cloudEvents, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(cloudEvents);

        foreach (var @event in cloudEvents)
        {
            await PublishAsync(@event, cancellationToken).ConfigureAwait(false);
        }
    }
}
