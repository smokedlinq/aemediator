namespace MediatR.Azure.EventGrid;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class EventGridDataTypeAttribute : Attribute
{
    public string Type { get; }
    public string? Version { get; }

    public EventGridDataTypeAttribute(string type, string? version = null)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Version = version;
    }
}
