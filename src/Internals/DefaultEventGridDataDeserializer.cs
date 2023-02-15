using System.Text.Json;
using MediatR.Azure.EventGrid.Serialization;

namespace MediatR.Azure.EventGrid.Internals;

internal sealed class DefaultEventGridDataDeserializer : EventGridDataDeserializer
{
    private readonly EventGridDataDeserializerOptions _options;

    public DefaultEventGridDataDeserializer(EventGridDataDeserializerOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public override object? Deserialize(Type type, BinaryData data)
        => JsonSerializer.Deserialize(data, type, _options.JsonSerializerOptions);
}
