namespace Board.Application.Abstractions.Services;
public interface ICurrentUserProvider
{
    public string GetCurrentUserEmail();
    public bool IsCurrentUserAdmin();

}
