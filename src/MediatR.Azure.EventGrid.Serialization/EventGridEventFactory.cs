using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace MediatR.Azure.EventGrid.Serialization;

/// <summary>
/// Represents a factory for creating <see cref="EventGridEvent"/> instances.
/// </summary>
public abstract class EventGridEventFactory
{
    /// <summary>
    /// Creates a new <see cref="EventGridEvent"/> instance.
    /// </summary>
    public abstract EventGridEvent Create<T>(string subject, T data);
}
