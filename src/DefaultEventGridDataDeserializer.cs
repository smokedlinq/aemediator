using System.Text.Json;

namespace MediatR.Azure.EventGrid;

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
