namespace BuildingBlocks.Pagination;

public record PaginationRequest
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }

    private const int MaxPageSize = 100;
    private const int DefaultPageSize = 10;
    private const int DefaultPageIndex = 1;

    public PaginationRequest(int pageIndex = DefaultPageIndex, int pageSize = DefaultPageSize)
    {
        PageIndex = pageIndex < 0 ? DefaultPageIndex : pageIndex;

        PageSize = pageSize switch
        {
            <= 0 => DefaultPageSize,
            > MaxPageSize => MaxPageSize,
            _ => pageSize
        };
    }
}