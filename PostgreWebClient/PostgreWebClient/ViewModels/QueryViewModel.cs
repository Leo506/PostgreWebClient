using PostgreWebClient.Models;

namespace PostgreWebClient.ViewModels;

public class QueryViewModel
{
    public QueryModel QueryModel { get; set; } = null!;

    public ErrorModel? ErrorModel { get; set; }

    public PaginationModel PaginationModel { get; set; } = null!;

    public DatabaseInfo DatabaseInfoModel { get; set; } = null!;
}