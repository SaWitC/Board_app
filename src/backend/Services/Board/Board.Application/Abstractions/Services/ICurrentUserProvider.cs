namespace Board.Application.Abstractions.Services;
public interface ICurrentUserProvider
{
    public string GetUserEmail();
    public bool IsGlobalAdmin();
}
