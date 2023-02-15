using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid;

/// <summary>
/// Represents an attribute that specifies the data type of an <see cref="EventGridEvent"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class EventGridDataTypeAttribute : Attribute
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
    /// Initializes a new instance of the <see cref="EventGridDataTypeAttribute"/> class.
    /// </summary>
    public EventGridDataTypeAttribute(string type, string? version = null)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Version = version;
    }
}
