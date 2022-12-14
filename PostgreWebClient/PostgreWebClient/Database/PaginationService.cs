using System.Data;
using Calabonga.OperationResults;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class PaginationService : IPaginationService
{
    private const string QueryToCount = "SELECT COUNT(*) FROM ({0}) as TmpTable";
    private readonly ICommandService _command;

    public PaginationService(ICommandService command)
    {
        _command = command;
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
            var totalCount = (long)_command.ExecuteCommand(string.Format(QueryToCount, query), connection).Rows![0][0];
            paginationModel.TotalCount = totalCount;
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