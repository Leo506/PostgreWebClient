using Microsoft.AspNetCore.SignalR;
using Npgsql;
using PostgreWebClient.Abstractions;

namespace PostgreWebClient;

public class ManipulationHub : Hub
{
    private readonly IDatabaseInfoService _databaseInfoService;
    private readonly IConnectionService _connectionService;

    public ManipulationHub(IDatabaseInfoService databaseInfoService, IConnectionService connectionService)
    {
        _databaseInfoService = databaseInfoService;
        _connectionService = connectionService;
    }

    public async Task ExecuteQuery(string query, string sessionId)
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

    public async Task GetDatabaseInfo(string sessionId)
    {
        var connection = _connectionService.Connections[sessionId];
        var databaseInfo = _databaseInfoService.GetDatabaseInfo(connection);
        await Clients.Caller.SendAsync("getDatabaseInfo", databaseInfo);
    }
}