using FastEndpoints;

namespace Board.Api.Features.Users.SearchUsers;

public class SearchUsersEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/users");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string query = Query<string>("q") ?? string.Empty;

        List<UserLookupDto> users = new List<UserLookupDto>
        {
            new() { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Email = "user1@example.com" },
            new() { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Email = "user2@example.com" },
            new() { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Email = "admin1@example.com" },
            new() { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), Email = "admin2@example.com" },
            new() { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Email = "developer1@example.com" },
            new() { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Email = "tester1@example.com" }
        };

        var result = users
            .Where(u => u.Email.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Select(u => new { id = u.Id.ToString(), email = u.Email })
            .ToList();

        await Send.OkAsync(result, ct);
    }

    private sealed class UserLookupDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
