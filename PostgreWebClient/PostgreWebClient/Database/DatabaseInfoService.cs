using Npgsql;
using PostgreWebClient.Abstractions;
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

    private readonly IExecutorFactory _factory;

    public DatabaseInfoService(IExecutorFactory factory)
    {
        _factory = factory;
    }

    public DatabaseInfo GetDatabaseInfo(NpgsqlConnection connection)
    {
        var info = new DatabaseInfo()
        {
            Schemas = new List<SchemaModel>()
        };

        try
        {
            var executor = _factory.GetExecutor(QueryToGetAllSchemas, connection);
            
            var schemasTable = executor.Execute();
            foreach (var row in schemasTable.Rows!)
            {
                info.Schemas.Add(new SchemaModel()
                {
                    SchemaName = row[0].ToString()!,
                    Tables = new List<string>()
                });
            }
            

            foreach (var schema in info.Schemas)
            {
                executor =
                    _factory.GetExecutor(string.Format(QueryToGetTables, schema.SchemaName), connection);
                
                var resultTable = executor.Execute();
                foreach (var row in resultTable.Rows!)
                {
                    schema.Tables.Add(row[0].ToString()!);
                }
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return info;
    }
}