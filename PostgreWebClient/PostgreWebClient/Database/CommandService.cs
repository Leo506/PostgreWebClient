using System.Data;
using Calabonga.OperationResults;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class CommandService : ICommandService
{
    private readonly ITableExtractor _extractor;

    public CommandService(ITableExtractor extractor) => _extractor = extractor;

    public Table ExecuteCommand(string query, IDbConnection connection)
    {
        try
        {
            var command = connection.CreateCommand();
            command.CommandText = query;
            using var reader = command.ExecuteReader();

            var result = _extractor.ExtractTable(reader);

            return result.Equals(Table.Empty) ? Table.SuccessResult(query) : result;
        }
        catch (Exception e)
        {
            return Table.ErrorResult(query, e.Message);
        }
    }
}