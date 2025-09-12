using System.Text.Json.Serialization;

namespace Board.Domain.Contracts.Models.HRM;

public sealed class KeyCloakTokenModel
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}
