using System.Text.Json;

namespace MediatR.Azure.EventGrid;

internal sealed class DefaultEventGridDataDeserializer : EventGridDataDeserializer
{
    private readonly JsonSerializerOptions _options;

    public DefaultEventGridDataDeserializer()
        : this(new JsonSerializerOptions())
    {
    }

    public DefaultEventGridDataDeserializer(JsonSerializerOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public override object? Deserialize(Type type, BinaryData data)
        => JsonSerializer.Deserialize(data, type, _options);
}
