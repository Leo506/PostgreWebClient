using System.Data;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class PaginationService
{
    private const string QueryToCount = "SELECT COUNT(*) FROM ({0}) as TmpTable";
    private readonly ICommandExecutor _executor;

    public PaginationService(ICommandExecutor executor)
    {
        _executor = executor;
    }

    public string Paginate(string query, PaginationModel paginationModel, IDbConnection connection)
    {
        var totalCount = (int)_executor.Execute(QueryToCount, connection).Rows![0][0];
        paginationModel.TotalRecordsCount = totalCount;
        return
            $"SELECT * FROM ({query}) as TmpTable OFFSET {(paginationModel.CurrentPage - 1) * PaginationModel.PageSize} " +
            $"LIMIT {PaginationModel.PageSize}";
    }
}