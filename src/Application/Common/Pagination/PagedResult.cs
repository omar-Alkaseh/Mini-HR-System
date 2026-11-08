namespace Application.Common.Pagination;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount)
{
    public int TotalPages =>
        (int)Math.Ceiling((decimal)TotalCount / PageSize);

    public bool HasPreviousPage() => PageNumber > 1;

    public bool HasNextPage() => PageNumber < TotalPages;
}