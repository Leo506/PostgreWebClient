using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class CommandService : ICommandService
{
    private readonly IExecutorFactory _factory;

    public CommandService(IExecutorFactory factory)
    {
        _factory = factory;
    }

    public QueryModel ExecuteCommand(QueryModel query, NpgsqlConnection connection)
    {
        var result = new QueryModel()
        {
            Headers = new List<string>(),
            Rows = new List<List<object>>(),
            QueryText = query.QueryText,
            Pagination = new PaginationModel()
            {
                CurrentPage = query.Pagination!.CurrentPage,
                TotalRecordsCount = 0
            }
        };

        try
        {
            var executor = _factory.GetExecutor(PrepareQuery(query.QueryText, query.Pagination), connection);
            var table = executor.Execute();

            if (table.Columns != null) result.Headers.AddRange(table.Columns);

            if (table.Rows == null) return result;
            
            foreach (var row in table.Rows)
            {
                result.Rows.Add(row);
            }

            executor = _factory.GetExecutor(GetQueryToCount(query.QueryText), connection);
            result.Pagination!.TotalRecordsCount = (long)(executor.Execute().Rows?[0][0] ?? 0);

            return result;
        }
        catch (Exception e)
        {
            result.HasError = true;
            result.ErrorText = e.Message;
        }

        return result;
    }


    private static string PrepareQuery(string queryText, PaginationModel? pagination)
    {
        if (queryText.EndsWith(';')) queryText = queryText.Replace(";", "");
        return
            $"SELECT * FROM ({queryText}) as tmpTable LIMIT {PaginationModel.PageSize} " +
            $"OFFSET {(pagination?.CurrentPage == 0 ? 0 : pagination!.CurrentPage - 1) * PaginationModel.PageSize}";
    }

    private static string GetQueryToCount(string queryText)
    {
        if (queryText.EndsWith(';')) queryText = queryText.Replace(";", "");
        return $"SELECT COUNT(*) FROM ({queryText}) as tmpTable";
    }
}