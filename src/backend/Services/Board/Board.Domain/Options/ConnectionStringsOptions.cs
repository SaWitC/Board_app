namespace Board.Domain.Options;

public sealed record ConnectionStringsOptions : IBoardOptions
{
    public static string SectionName => "ConnectionStrings";
    public string BoardDbConnectionString { get; init; }
}
