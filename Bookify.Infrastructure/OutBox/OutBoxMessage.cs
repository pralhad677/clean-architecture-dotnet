namespace Bookify.Infrastructure.OutBox;

public sealed class OutBoxMessage
{
    public OutBoxMessage(Guid id, DateTime occuredOn, string type, string content)
    {
        Id = id;
        OccuredOn = occuredOn;
        Type = type;
        Content = content;
    }

    public Guid Id { get; init; }
    public DateTime OccuredOn { get; init; }
    public string Type { get; init; }
    public string Content { get; init; }
    public DateTime? ProcessedOnUtc { get; init; }
    public string? Error { get; init; }
}