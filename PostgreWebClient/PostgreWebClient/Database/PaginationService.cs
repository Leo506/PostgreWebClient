using System.Data;
using PostgreWebClient.Factories;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class PaginationService
{
    private const string QueryToCount = "SELECT COUNT(*) FROM ({0}) as TmpTable";
    
    public string Paginate(string query, PaginationModel paginationModel, IDbConnection connection)
    {
        var executor = NpgsqlExecutorFactory.GetExecutor(string.Format(QueryToCount, query), connection);
        var totalCount = (int)executor.Execute().Rows![0][0];
        paginationModel.TotalRecordsCount = totalCount;
        return
            $"SELECT * FROM ({query}) as TmpTable OFFSET {(paginationModel.CurrentPage - 1) * PaginationModel.PageSize} " +
            $"LIMIT {PaginationModel.PageSize}";
    }
}