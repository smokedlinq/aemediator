namespace MediatR.Azure.EventGrid;

public sealed class EventGridDataTypeRegistration
{
    public EventGridDataType DataType { get; }
    public Type Type { get; }

    public EventGridDataTypeRegistration(EventGridDataType dataType, Type type)
    {
        DataType = dataType ?? throw new ArgumentNullException(nameof(dataType));
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}
