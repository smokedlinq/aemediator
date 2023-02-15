using System.Runtime.Serialization;

namespace MediatR.Azure.EventGrid;

[Serializable]
public class EventGridDataTypeNotFoundException : Exception
{
    public EventGridDataTypeNotFoundException()
        : base($"Could not find type for event data.")
    {
    }

    protected EventGridDataTypeNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
