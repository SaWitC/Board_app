using System.Text.Json.Serialization;

namespace Board.Domain.Contracts.Models.HRM;

public sealed class EmployeeModel
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    
    [JsonPropertyName("firstNameRu")]
    public string? FirstNameRU { get; init; }
    
    [JsonPropertyName("patronymicRu")]
    public string? PatronymicRU { get; init; }
    
    [JsonPropertyName("lastNameRu")]
    public string? LastNameRU { get; init; }
    
    [JsonPropertyName("firstNameEn")]
    public string? FirstNameEN { get; init; }
    
    [JsonPropertyName("patronymicEn")]
    public string? PatronymicEN { get; init; }
    
    [JsonPropertyName("lastNameEn")]
    public string? LastNameEN { get; init; }

    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [JsonPropertyName("roomId")]
    public string? Department { get; init; }


    [JsonPropertyName("linkProfilePictureMini")]
    public string? LinkProfilePictureMini { get; set; }
}
