using Board.Application.Abstractions.Repositories;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Security;
using FastEndpoints;

namespace Board.Api.Features.BoardColumn.UpdateBoardColumnOrder;

public class UpdateBoardColumnsOrderEndpoint : Endpoint<UpdateBoardColumnsOrderRequest>
{
    private readonly IBoardColumnRepository _boardColumnRepository;
    public UpdateBoardColumnsOrderEndpoint(IBoardColumnRepository boardColumnRepository)
    {
        _boardColumnRepository = boardColumnRepository;
    }

    public override void Configure()
    {
        Put("/api/boards/{boardId}/columns/order");
        Policies(Auth.BuildPermissionPolicy(Permission.ManageColumns, Context.BoardColumn, "boardId"));
    }

    public override async Task HandleAsync(UpdateBoardColumnsOrderRequest request, CancellationToken cancellationToken)
    {
        Guid boardId = Route<Guid>("boardId");

        IList<Domain.Entities.BoardColumn> columns = await _boardColumnRepository.GetByBoardIdAsync(boardId, cancellationToken);

        foreach (Application.DTOs.OrderedColumnDto updatedColumns in request.Columns ?? [])
        {
            columns.First(x => x.Id == updatedColumns.Id).Order = updatedColumns.Order;
        }

        await _boardColumnRepository.SaveChangesAsync(cancellationToken);
    }
}
