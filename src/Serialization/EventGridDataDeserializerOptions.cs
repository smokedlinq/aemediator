using System.Text.Json;
using MediatR.Azure.EventGrid.Internals;

namespace MediatR.Azure.EventGrid.Serialization;

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
