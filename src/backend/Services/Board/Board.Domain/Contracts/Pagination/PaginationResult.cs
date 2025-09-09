namespace Board.Domain.Contracts.Pagination;

public sealed class PaginationResult<TModel>
{
    public required ICollection<TModel> Items { get; init; }

    public required int TotalCount { get; init; }
}
