using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using MediatR;

namespace Board.Application.Queries.BoardColumns.GetBoardColumnById;

public class GetBoardColumnByIdQueryHandler : IRequestHandler<GetBoardColumnByIdQuery, BoardColumnDto>
{
	private readonly IRepository<BoardColumn> _repository;

	public GetBoardColumnByIdQueryHandler(IRepository<BoardColumn> repository)
	{
		_repository = repository;
	}

	public async Task<BoardColumnDto> Handle(GetBoardColumnByIdQuery request, CancellationToken cancellationToken)
	{
		var entity = await _repository.GetAsync(x => x.Id == request.Id, cancellationToken);
        return entity == null
            ? null
            : new BoardColumnDto
                  {
                      Id = entity.Id,
                      Title = entity.Title,
                      Description = entity.Description
                  };
    }
} 
