namespace PostgreWebClient.Models;

public class PaginationModel
{
    public const int PageSize = 10;

    public int CurrentPage { get; set; }

    public  long TotalCount { get; set; }
}