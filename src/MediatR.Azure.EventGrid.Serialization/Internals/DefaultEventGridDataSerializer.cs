using System.Text.Json;

namespace MediatR.Azure.EventGrid.Serialization.Internals;

internal sealed class DefaultEventGridDataSerializer : EventGridDataSerializer
{
    private readonly EventGridDataSerializerOptions _options;

    public DefaultEventGridDataSerializer(EventGridDataSerializerOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public override object? Deserialize(Type type, BinaryData data)
        => JsonSerializer.Deserialize(data, type, _options.JsonSerializerOptions);

    public override BinaryData Serialize<T>(T data)
        => BinaryData.FromObjectAsJson(data, _options.JsonSerializerOptions);
}
