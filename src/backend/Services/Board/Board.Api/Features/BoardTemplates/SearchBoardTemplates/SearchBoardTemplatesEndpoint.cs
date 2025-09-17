using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardTemplates.SearchBoardTemplates;

public class SearchBoardTemplatesEndpoint : Endpoint<SearchBoardTemplatesRequest>
{
    private readonly MapsterMapper.IMapper _mapper;
    private readonly IBoardTemplateRepository _repository;

    public SearchBoardTemplatesEndpoint(IBoardTemplateRepository repository, MapsterMapper.IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public override void Configure()
    {
        Get("/api/boardtemplates/search");
    }

    public override async Task HandleAsync(SearchBoardTemplatesRequest request, CancellationToken cancellationToken)
    {
        int page = 1;
        int pageSize = 10;
        List<BoardTemplate> foundTemplates = await _repository.FindAsync(request.SearchTerm, page, pageSize, cancellationToken, x => x.Title, x => x.Description);

        BoardTemplateDto[] response = _mapper.Map<BoardTemplateDto[]>(foundTemplates);

        await Send.OkAsync(response, cancellationToken);
    }
}
