namespace Board.Domain.Options;

public sealed class AuthOptions : IBoardOptions
{
    public static string SectionName => "Auth";
	public required string Authority { get; init; }
	public required string Audience { get; init; }
} 
