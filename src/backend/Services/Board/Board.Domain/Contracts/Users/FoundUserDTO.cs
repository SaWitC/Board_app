namespace Board.Domain.Contracts.Users;
public class FoundUserDTO
{
    public required int Id { get; init; }
    public required string FirstNameEn { get; init; }
    public required string LastNameEn { get; init; }
    public string FirstNameRu { get; set; }
    public string LastNameRu { get; set; }
    public string LinkProfilePictureMini { get; set; }
}
