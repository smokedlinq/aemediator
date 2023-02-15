namespace MediatR.Azure.EventGrid;

public sealed class EventDataTypeRegistration
{
    public EventDataType DataType { get; }
    public Type Type { get; }

    public EventDataTypeRegistration(EventDataType dataType, Type type)
    {
        DataType = dataType ?? throw new ArgumentNullException(nameof(dataType));
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}
