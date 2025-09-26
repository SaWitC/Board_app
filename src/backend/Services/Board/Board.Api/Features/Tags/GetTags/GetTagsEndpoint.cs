using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs.Tags;
using FastEndpoints;

namespace Board.Api.Features.Tags.GetTags;

public class GetTagsEndpoint : Endpoint<GetTagsRequest, GetTagsResponse>
{
    private readonly ITagRepository _repository;

    public GetTagsEndpoint(ITagRepository repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Get("/api/tags");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTagsRequest request, CancellationToken ct)
    {
        var tags = await _repository.GetAllAsync(
            t => string.IsNullOrEmpty(request.SearchTerm) || t.Title.Contains(request.SearchTerm),
            ct,
            false
        );

        var response = new GetTagsResponse
        {
            Tags = tags.Select(t => new TagDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description
            }).ToList()
        };

        await Send.OkAsync(response, ct);
    }
}

public class GetTagsRequest
{
    public string SearchTerm { get; set; } = string.Empty;
}

public class GetTagsResponse
{
    public List<TagDto> Tags { get; set; } = new();
} 