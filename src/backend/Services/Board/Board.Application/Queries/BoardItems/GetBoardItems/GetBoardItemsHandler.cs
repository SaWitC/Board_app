namespace Board.Application.Queries.BoardItems.GetBoardItems;

	using Board.Application.DTOs;
	using Board.Application.Repositories;
	using MediatR;

	public class GetBoardItemsHandler : IRequestHandler<GetBoardItemsQuery, List<BoardItemDto>>
	{
		private readonly IBoardItemRepository _repository;

		public GetBoardItemsHandler(IBoardItemRepository repository)
		{
			_repository = repository;
		}

		public async Task<List<BoardItemDto>> Handle(GetBoardItemsQuery request, CancellationToken cancellationToken)
		{
			var items = await _repository.GetAllAsync(cancellationToken);
			return items.Select(e => new BoardItemDto
			{
				Id = e.Id,
				Title = e.Title,
				Description = e.Description,
				BoardColumnId = e.BoardColumnId,
				Priority = e.Priority,
				AssigneeId = e.AssigneeId,
				DueDate = e.DueDate,
				ModificationDate = e.ModificationDate,
				CreatedTime = e.CreatedTime
			}).ToList();
		}
	}
