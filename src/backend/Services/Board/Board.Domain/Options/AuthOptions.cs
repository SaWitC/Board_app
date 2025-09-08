namespace Board.Domain.Options;

public sealed record AuthOptions : IBoardOptions
{
    public static string SectionName => "Auth";
    public string Authority { get; init; }
    public string Audience { get; init; }
    public bool IsBypassAuthorization { get; init; }
} 