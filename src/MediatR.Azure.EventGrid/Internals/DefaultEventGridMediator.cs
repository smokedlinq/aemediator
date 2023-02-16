using System.Diagnostics.CodeAnalysis;
using Azure.Messaging;
using Azure.Messaging.EventGrid;
using MediatR.Azure.EventGrid.Serialization;

namespace MediatR.Azure.EventGrid.Internals;

internal sealed class DefaultEventGridMediator : EventGridMediator
{
    private readonly IMediator _mediator;
    private readonly EventGridDataTypeResolver _dataTypeResolver;
    private readonly EventGridDataSerializer _dataDeserializer;

    public DefaultEventGridMediator(IMediator mediator, EventGridDataTypeResolver dataTypeResolver, EventGridDataSerializer dataDeserializer)
    {
        _mediator = mediator;
        _dataTypeResolver = dataTypeResolver;
        _dataDeserializer = dataDeserializer;
    }

    public override async Task PublishAsync(EventGridEvent eventGridEvent, CancellationToken cancellationToken = default)
    {
        _ = eventGridEvent ?? throw new ArgumentNullException(nameof(eventGridEvent));
        await PublishAsync(eventGridEvent, eventGridEvent.Data, cancellationToken).ConfigureAwait(false);
    }

    public override async Task PublishAsync(CloudEvent cloudEvent, CancellationToken cancellationToken = default)
    {
        _ = cloudEvent ?? throw new ArgumentNullException(nameof(cloudEvent));
        await PublishAsync(cloudEvent, cloudEvent.Data, cancellationToken).ConfigureAwait(false);
    }

    private async Task PublishAsync<T>(T @event, BinaryData? data, CancellationToken cancellationToken)
        where T : class
    {
        var type = _dataTypeResolver.Resolve(@event);
        var eventData = _dataDeserializer.Deserialize(@event, type, data);

        type ??= eventData?.GetType() ?? throw new EventGridDataTypeNotFoundException();

        var notification = EventGridNotificationFactory.Create(@event, type, eventData);

        await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
    }
}
