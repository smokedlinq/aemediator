using System.Collections.Concurrent;
using System.Net.Mime;
using Azure.Messaging;

namespace MediatR.Azure.EventGrid.Serialization.Internals;

internal sealed class DefaultCloudEventFactory : CloudEventFactory
{
    private readonly EventGridDataTypeResolver _resolver;
    private readonly EventGridDataSerializer _serializer;

    public DefaultCloudEventFactory(EventGridDataTypeResolver resolver, EventGridDataSerializer serializer)
    {
        _resolver = resolver;
        _serializer = serializer;
    }

    public override CloudEvent Create<T>(string source, T data)
    {
        var dataType = _resolver.Resolve(typeof(T)) ?? throw new EventGridDataTypeNotFoundException();
        var eventData = _serializer.Serialize(data);

        return new CloudEvent(source, dataType.Type, eventData, MediaTypeNames.Application.Json, dataFormat: CloudEventDataFormat.Json)
        {
            DataSchema = dataType.Version,
        };
    }
}
