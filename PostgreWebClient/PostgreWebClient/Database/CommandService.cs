using System.Data;
using Calabonga.OperationResults;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class CommandService : ICommandService
{
    public Table ExecuteCommand(string query, IDbConnection connection)
    {
        try
        {
            var result = new Table();
            var command = connection.CreateCommand();
            command.CommandText = query;
            using var reader = command.ExecuteReader();
            
            result.Columns = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToList();
            result.Rows = new List<List<object>>();
                
            while (reader.Read())
            {
                result.Rows.Add(Enumerable.Range(0, reader.FieldCount).Select(i => reader[i]).ToList());
            }

            return result.Equals(Table.Empty) ? Table.SuccessResult(query) : result;
        }
        catch (Exception e)
        {
            return Table.ErrorResult(query, e.Message);
        }
    }
}