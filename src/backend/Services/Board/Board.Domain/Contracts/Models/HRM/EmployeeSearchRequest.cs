namespace Board.Domain.Contracts.Models.HRM;

public sealed class EmployeeSearchRequest
{
    public required string NameOrSurname { get; init; }
}
