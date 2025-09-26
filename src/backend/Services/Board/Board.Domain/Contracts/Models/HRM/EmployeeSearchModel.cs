using System.Text.Json.Serialization;

namespace Board.Domain.Contracts.Models.HRM;

public sealed class EmployeeSearchModel
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [JsonPropertyName("firstNameEn")]
    public required string FirstNameEn { get; init; }

    [JsonPropertyName("lastNameEn")]
    public required string LastNameEn { get; init; }

    [JsonPropertyName("firstNameRu")]
    public string FirstNameRu { get; set; }

    [JsonPropertyName("lastNameRu")]
    public string LastNameRu { get; set; }

    [JsonPropertyName("linkProfilePictureMini")]
    public string LinkProfilePictureMini { get; set; }
}
