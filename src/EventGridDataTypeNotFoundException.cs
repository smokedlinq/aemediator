using System.Runtime.Serialization;

namespace MediatR.Azure.EventGrid;

/// <summary>
/// Represents an exception that is thrown when the .NET type for event data could not be found.
/// </summary>
[Serializable]
public class EventGridDataTypeNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventGridDataTypeNotFoundException"/> class.
    /// </summary>
    public EventGridDataTypeNotFoundException()
        : base($"Could not find type for event data.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventGridDataTypeNotFoundException"/> class with the serialized data.
    /// </summary>
    protected EventGridDataTypeNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
