#pragma warning disable S3925 // "ISerializable" should be implemented correctly

namespace MediatR.Azure.EventGrid.Serialization;

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
}
