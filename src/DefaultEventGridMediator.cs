using System.Diagnostics.CodeAnalysis;
using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

internal sealed class DefaultEventGridMediator : EventGridMediator
{
    private readonly IMediator _mediator;
    private readonly EventDataTypeResolver _dataTypeResolver;
    private readonly EventDataDeserializer _dataDeserializer;

    public DefaultEventGridMediator(IMediator mediator, EventDataTypeResolver dataTypeResolver, EventDataDeserializer dataDeserializer)
    {
        _mediator = mediator;
        _dataTypeResolver = dataTypeResolver;
        _dataDeserializer = dataDeserializer;
    }

    public override async Task PublishAsync(EventGridEvent eventGridEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventGridEvent);
        await PublishAsync(eventGridEvent, eventGridEvent.Data, cancellationToken).ConfigureAwait(false);
    }

    public override async Task PublishAsync(CloudEvent cloudEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(cloudEvent);
        await PublishAsync(cloudEvent, cloudEvent.Data, cancellationToken).ConfigureAwait(false);
    }

    private async Task PublishAsync<T>(T @event, BinaryData? data, CancellationToken cancellationToken)
        where T : class
    {
        var type = _dataTypeResolver.Resolve(@event);
        var eventData = _dataDeserializer.Deserialize(@event, type, data);

        type ??= eventData?.GetType() ?? throw new EventDataTypeNotFoundException();

        var notification = EventGridNotificationFactory.Create(@event, type, eventData);

        await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
    }
}
