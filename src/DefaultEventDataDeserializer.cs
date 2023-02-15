using System.Text.Json;

namespace MediatR.Azure.EventGrid;

internal sealed class DefaultEventDataDeserializer : EventDataDeserializer
{
    private readonly JsonSerializerOptions _options;

    public DefaultEventDataDeserializer()
        : this(new JsonSerializerOptions())
    {
    }

    public DefaultEventDataDeserializer(JsonSerializerOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public override object? Deserialize(Type type, BinaryData data)
        => JsonSerializer.Deserialize(data, type, _options);
}
