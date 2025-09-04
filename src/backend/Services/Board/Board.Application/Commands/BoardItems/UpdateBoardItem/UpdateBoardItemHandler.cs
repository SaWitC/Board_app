namespace Board.Application.Commands.BoardItems.UpdateBoardItem;

	using Board.Application.DTOs;
	using Board.Application.Repositories;
	using MediatR;

	public class UpdateBoardItemHandler : IRequestHandler<UpdateBoardItemCommand, BoardItemDto>
	{
		private readonly IBoardItemRepository _repository;

		public UpdateBoardItemHandler(IBoardItemRepository repository)
		{
			_repository = repository;
		}

		public async Task<BoardItemDto> Handle(UpdateBoardItemCommand request, CancellationToken cancellationToken)
		{
			var entity = await _repository.GetAsync(request.Id, cancellationToken);
			if (entity == null)
			{
				return null;
			}

			entity.Title = request.Title;
			entity.Description = request.Description;
			entity.BoardColumnId = request.BoardColumnId;
			entity.Priority = request.Priority;
			entity.AssigneeId = request.AssigneeId;
			entity.DueDate = request.DueDate;
			entity.ModificationDate = DateTimeOffset.UtcNow;

			var updated = await _repository.UpdateAsync(entity, cancellationToken);
			return new BoardItemDto
			{
				Id = updated.Id,
				Title = updated.Title,
				Description = updated.Description,
				BoardColumnId = updated.BoardColumnId,
				Priority = updated.Priority,
				AssigneeId = updated.AssigneeId,
				DueDate = updated.DueDate,
				ModificationDate = updated.ModificationDate,
				CreatedTime = updated.CreatedTime
			};
		}
	}
