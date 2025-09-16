using Board.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;

namespace Board.Application.Services;
public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetCurrentUserEmail()
    {
        return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
    }

    public bool IsCurrentUserAdmin()
    {
        return _httpContextAccessor.HttpContext.User.IsInRole("Admin");
    }
}
