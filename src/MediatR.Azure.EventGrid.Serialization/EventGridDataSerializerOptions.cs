using System.Text.Json;

namespace MediatR.Azure.EventGrid.Serialization;

/// <summary>
/// Represents options for the <see cref="EventGridDataSerializerOptions"/>.
/// </summary>
public class EventGridDataSerializerOptions
{
    /// <summary>
    /// Gets the <see cref="JsonSerializerOptions"/> to use when deserializing event data.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; } = new();
}
