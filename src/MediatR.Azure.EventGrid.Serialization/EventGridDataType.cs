namespace MediatR.Azure.EventGrid.Serialization;

/// <summary>
/// Represents the data type of an Event Grid event.
/// </summary>
public sealed record EventGridDataType
{
    /// <summary>
    /// Gets the data type of the event.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the version of the data type of the event.
    /// </summary>
    public string? Version { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventGridDataType"/> class.
    /// </summary>
    public EventGridDataType(string type, string? version)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Version = version;
    }
}
