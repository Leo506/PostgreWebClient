using Calabonga.OperationResults;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Factories;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class DatabaseInfoService : IDatabaseInfoService
{
    private const string QueryToGetAllSchemas = "select schema_name " +
                                                "from information_schema.schemata";

    private const string QueryToGetTables = "select table_name " +
                                            "from information_schema.tables " +
                                            "where table_schema = '{0}' and " +
                                            "table_type = 'BASE TABLE'";

    private const string QueryToGetViews = "select table_name " +
                                           "from information_schema.tables " +
                                           "where table_schema = '{0}' and " +
                                           "table_type = 'VIEW'";
    

    public OperationResult<DatabaseInfo> GetDatabaseInfo(NpgsqlConnection connection)
    {
        var result = OperationResult.CreateResult<DatabaseInfo>();
        result.Result = new DatabaseInfo()
        {
            Schemas = new List<SchemaModel>()
        };

        try
        {
            var executor = NpgsqlExecutorFactory.GetExecutor(QueryToGetAllSchemas, connection);
            
            var schemasTable = executor.Execute();
            foreach (var row in schemasTable.Rows!)
            {
                result.Result.Schemas.Add(new SchemaModel()
                {
                    SchemaName = row[0].ToString()!,
                    Tables = new List<string>()
                });
            }
            

            foreach (var schema in result.Result.Schemas)
            {
                executor =
                    NpgsqlExecutorFactory.GetExecutor(string.Format(QueryToGetTables, schema.SchemaName), connection);
                
                var resultTable = executor.Execute();
                foreach (var row in resultTable.Rows!)
                {
                    schema.Tables.Add(row[0].ToString()!);
                }

                executor = NpgsqlExecutorFactory.GetExecutor(string.Format(QueryToGetViews, schema.SchemaName), connection);
                resultTable = executor.Execute();
                if (resultTable.Rows == null || resultTable.Rows.Count == 0) continue;
                schema.Views = new List<string>();
                foreach (var row in resultTable.Rows)
                {
                    schema.Views.Add(row[0].ToString()!);
                }
            }
        }
        catch (Exception e)
        {
            result.AddError(e);
        }

        return result;
    }
}