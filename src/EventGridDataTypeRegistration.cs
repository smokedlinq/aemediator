namespace MediatR.Azure.EventGrid;

/// <summary>
/// Represents a dependency service registration of a data type for an <see cref="EventGridDataType"/>.
/// </summary>
public sealed class EventGridDataTypeRegistration
{
    /// <summary>
    /// Gets the <see cref="EventGridDataType"/>.
    /// </summary>
    public EventGridDataType DataType { get; }

    /// <summary>
    /// Gets the .NET type.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventGridDataTypeRegistration"/> class.
    /// </summary>
    public EventGridDataTypeRegistration(EventGridDataType dataType, Type type)
    {
        DataType = dataType ?? throw new ArgumentNullException(nameof(dataType));
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}
