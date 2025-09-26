using System.Security.Claims;
using Board.Application.Abstractions.Services;
using Board.Domain.Security;
using Microsoft.AspNetCore.Http;

namespace Board.Application.Services;
public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetUserEmail()
    {
        return User?.FindFirst(ClaimTypes.Email)?.Value;
    }

    public bool IsGlobalAdmin()
    {
        return User?.HasClaim(Auth.Claims.Permissions, Auth.Roles.GlobalAdmin) ?? false;
    }
}
