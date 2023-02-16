using Azure.Messaging;

namespace MediatR.Azure.EventGrid.Serialization;

/// <summary>
/// Represents a factory for creating <see cref="CloudEvent"/> instances.
/// </summary>
public abstract class CloudEventFactory
{
    /// <summary>
    /// Creates a new <see cref="CloudEvent"/> instance.
    /// </summary>
    public abstract CloudEvent Create<T>(string source, T data);
}
