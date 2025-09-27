using System.Text.RegularExpressions;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Board.Api.Authorization;

public sealed class BoardPermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
    private static readonly Regex PolicyRegex = new(
        $"^{Auth.Claims.Permissions}:(?<perm>[^:]+):(?<ctx>[^:]+):(?<route>[^:]+)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public BoardPermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => _fallbackPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        Match match = PolicyRegex.Match(policyName);
        if (!match.Success)
        {
            return _fallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        string permStr = match.Groups["perm"].Value;
        string ctxStr = match.Groups["ctx"].Value;
        string routeKey = match.Groups["route"].Value;

        if (!Enum.TryParse(permStr, ignoreCase: true, out Permission perm))
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }
        if (!Enum.TryParse(ctxStr, ignoreCase: true, out Context ctx))
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }

        AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
            .AddRequirements(new BoardPermissionRequirement(perm, ctx, routeKey))
            .Build();

        return Task.FromResult(policy);
    }
}
