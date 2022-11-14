using System.Data;
using Calabonga.OperationResults;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class PaginationService : IPaginationService
{
    private const string QueryToCount = "SELECT COUNT(*) FROM ({0}) as TmpTable";
    private readonly ICommandExecutor _executor;

    public PaginationService(ICommandExecutor executor)
    {
        _executor = executor;
    }

    public OperationResult<string> Paginate(string query, PaginationModel paginationModel, IDbConnection connection)
    {
        var result = OperationResult.CreateResult<string>();
        if (!query.ToUpper().StartsWith("SELECT"))
        {
            result.Result = query;
            return result;
        }

        query = PrepareQuery(query);
        try
        {
            var totalCount = (long)_executor.Execute(string.Format(QueryToCount, query), connection).Rows![0][0];
            paginationModel.TotalRecordsCount = totalCount;
            result.Result =
                $"SELECT * FROM ({query}) as TmpTable OFFSET {(paginationModel.CurrentPage - 1) * PaginationModel.PageSize} " +
                $"LIMIT {PaginationModel.PageSize}";
        }
        catch (Exception e)
        {
            result.AddError(e);
        }

        return result;
        
    }

    private string PrepareQuery(string query) => query.Replace(";", "");
}