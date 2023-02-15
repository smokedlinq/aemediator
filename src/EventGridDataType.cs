namespace MediatR.Azure.EventGrid;

public sealed record EventGridDataType
{
    public string Type { get; }
    public string? Version { get; }

    public EventGridDataType(string type, string? version)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Version = version;
    }
}
