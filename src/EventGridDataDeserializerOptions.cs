using System.Text.Json;

namespace MediatR.Azure.EventGrid;

/// <summary>
/// Represents options for the <see cref="DefaultEventGridDataDeserializer"/>.
/// </summary>
public class EventGridDataDeserializerOptions
{
    /// <summary>
    /// Gets the <see cref="JsonSerializerOptions"/> to use when deserializing event data.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; } = new();
}
