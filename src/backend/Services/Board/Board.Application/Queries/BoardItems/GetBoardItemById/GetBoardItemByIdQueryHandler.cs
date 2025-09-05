namespace Board.Application.Queries.BoardItems.GetBoardItemById;

	using Board.Application.DTOs;
	using Board.Application.Repositories;
	using MediatR;

	public class GetBoardItemByIdQueryHandler : IRequestHandler<GetBoardItemByIdQuery, BoardItemDto>
	{
		private readonly IBoardItemRepository _repository;

		public GetBoardItemByIdQueryHandler(IBoardItemRepository repository)
		{
			_repository = repository;
		}

		public async Task<BoardItemDto> Handle(GetBoardItemByIdQuery request, CancellationToken cancellationToken)
		{
			var entity = await _repository.GetAsync(request.Id, cancellationToken);
			if (entity == null)
			{
				return null;
			}

			return new BoardItemDto
			{
				Id = entity.Id,
				Title = entity.Title,
				Description = entity.Description,
				BoardColumnId = entity.BoardColumnId,
				Priority = entity.Priority,
				AssigneeId = entity.AssigneeId,
				DueDate = entity.DueDate,
				ModificationDate = entity.ModificationDate,
				CreatedTime = entity.CreatedTime
			};
		}
	}
