using System.Text.Json.Serialization;

namespace Board.Domain.Contracts.Models.HRM;

public sealed class EmployeeSearchResult
{
    [JsonPropertyName("totalElements")]
    public required int TotalElements { get; init; }

    [JsonPropertyName("content")]
    public required ICollection<EmployeeSearchModel> Content { get; init; }
}
