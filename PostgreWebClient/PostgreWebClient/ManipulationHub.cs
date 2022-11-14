using Microsoft.AspNetCore.SignalR;

namespace PostgreWebClient;

public class ManipulationHub : Hub
{
    public async Task ExecuteQuery(string query)
    {
        await Clients.Caller.SendAsync("getTable", new
        {
            Columns = new List<string>() { "column_1", "column_2" },
            Rows = new List<List<string>>()
            {
                new() { "value_1", "value_2" },
                new() { "value_3", "value_4" }
            }
        });
    }

    public async Task GetDatabaseInfo()
    {
        await Clients.Caller.SendAsync("getDatabaseInfo", new
        {
            Schemas = new List<object>()
            {
                new
                {
                    Name = "Schema1",
                    Tables = new List<string>()
                    {
                        "Table1",
                        "Table2",
                        "Table2"
                    }
                },
                new
                {
                    Name = "Schema2"
                }
            }
        });
    }
}