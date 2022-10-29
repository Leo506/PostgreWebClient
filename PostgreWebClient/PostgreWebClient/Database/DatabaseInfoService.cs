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
                                            "where table_schema = '{0}'";
    
    public DatabaseInfo GetDatabaseInfo(NpgsqlConnection connection)
    {
        var info = new DatabaseInfo()
        {
            Schemas = new List<SchemaModel>()
        };
        
        var schemaCmd = new NpgsqlCommand(QueryToGetAllSchemas, connection);
        var schemaReader = schemaCmd.ExecuteReader();
        while (schemaReader.Read())
        {
            info.Schemas.Add(new SchemaModel()
            {
                SchemaName = schemaReader.GetString(0),
                Tables = new List<string>()
            });
        }
        schemaReader.Close();
        schemaReader.Dispose();
        
        foreach (var schema in info.Schemas)
        {
            var cmd = new NpgsqlCommand(string.Format(QueryToGetTables, schema.SchemaName), connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                schema.Tables.Add(reader.GetString(0));
            }
            reader.Close();
            reader.Dispose();
        }

        return info;
    }
}