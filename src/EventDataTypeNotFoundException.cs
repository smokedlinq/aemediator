using System.Runtime.Serialization;

namespace MediatR.Azure.EventGrid;

[Serializable]
public class EventDataTypeNotFoundException : Exception
{
    public EventDataTypeNotFoundException()
        : base($"Could not find type for event data.")
    {
    }

    protected EventDataTypeNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
