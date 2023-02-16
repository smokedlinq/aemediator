using System.Collections.Concurrent;
using System.Net.Mime;
using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid.Serialization.Internals;

internal sealed class DefaultEventGridEventFactory : EventGridEventFactory
{
    private readonly EventGridDataTypeResolver _resolver;
    private readonly EventGridDataSerializer _serializer;

    public DefaultEventGridEventFactory(EventGridDataTypeResolver resolver, EventGridDataSerializer serializer)
    {
        _resolver = resolver;
        _serializer = serializer;
    }

    public override EventGridEvent Create<T>(string subject, T data)
    {
        var dataType = _resolver.Resolve(typeof(T)) ?? throw new EventGridDataTypeNotFoundException();
        var eventData = _serializer.Serialize(data);

        return new EventGridEvent(subject, dataType.Type, dataType.Version, eventData);
    }
}
