using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using MediatR;

namespace Board.Application.Commands.BoardColumns.UpdateBoardColumn;

public class UpdateBoardColumnHandler : IRequestHandler<UpdateBoardColumnCommand, BoardColumnDto>
{
	private readonly IRepository<BoardColumn> _repository;

	public UpdateBoardColumnHandler(IRepository<BoardColumn> repository)
	{
		_repository = repository;
	}

	public async Task<BoardColumnDto> Handle(UpdateBoardColumnCommand request, CancellationToken cancellationToken)
	{
		var entity = await _repository.GetAsync(x => x.Id == request.Id, cancellationToken, false);
		if (entity == null)
		{
			throw new InvalidOperationException("Column not found");
		}

		entity.Title = request.Title;
		entity.Description = request.Description;

		var updated = await _repository.UpdateAsync(entity, cancellationToken);

		return new BoardColumnDto
		{
			Id = updated.Id,
			Title = updated.Title,
			Description = updated.Description
		};
	}
} 