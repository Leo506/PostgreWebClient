namespace PostgreWebClient.Models;

public class PaginationModel
{
    public const int PageSize = 10;

    public int CurrentPage { get; set; }

    public  long TotalRecordsCount { get; set; }

    public bool HasNextPage() => (CurrentPage + 1) * PageSize <= TotalRecordsCount;

    public bool HasPreviousPage() => CurrentPage > 1;
}