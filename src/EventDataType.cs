namespace MediatR.Azure.EventGrid;

public sealed record EventDataType
{
    public string Type { get; }
    public string? Version { get; }

    public EventDataType(string type, string? version)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Version = version;
    }
}
