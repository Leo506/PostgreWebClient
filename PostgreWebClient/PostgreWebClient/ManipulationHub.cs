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
}