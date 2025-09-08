namespace Board.Domain.Contracts.Models.HRM;

public sealed class KeyCloakTokenRequestParameters
{
    [AliasAs("grant_type")]
    public string GrantType { get; } = "password";

    [AliasAs("client_id")]
    public required string ClientId { get; set; }

    [AliasAs("username")]
    public required string Username { get; set; }

    [AliasAs("password")]
    public required string Password { get; set; }
}
