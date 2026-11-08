namespace Application.Common.Pagination;

public sealed record PaginationParameters
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}