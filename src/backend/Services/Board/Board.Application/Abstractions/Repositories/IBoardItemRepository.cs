using Board.Application.DTOs.BoardItems;

namespace Board.Application.Abstractions.Repositories;
public interface IBoardItemRepository : IRepository<Domain.Entities.BoardItem>
{
    Task<ICollection<BoardItemLookupDto>> GetAllBoardItemsLookup(Guid boardId, CancellationToken cancellationToken);

}
