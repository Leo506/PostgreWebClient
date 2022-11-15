using Microsoft.AspNetCore.SignalR;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient;

public class ManipulationHub : Hub
{
    private readonly IDatabaseInfoService _databaseInfoService;
    private readonly IConnectionService _connectionService;
    private readonly ICommandService _commandService;

    public ManipulationHub(IDatabaseInfoService databaseInfoService, IConnectionService connectionService,
        ICommandService commandService)
    {
        _databaseInfoService = databaseInfoService;
        _connectionService = connectionService;
        _commandService = commandService;
    }

    public async Task ExecuteQuery(string query, string sessionId)
    {
        var connection = _connectionService.Connections[sessionId];
        var tableResult = _commandService.ExecuteCommand(query, connection);
        
        // TODO: remove table forming logic
        await Clients.Caller.SendAsync("getTable", tableResult.Ok && tableResult.Result!.Columns!.Count != 0 ? tableResult.Result : new Table()
        {
            Columns = new List<string>() {"Query", "Result"},
            Rows = new List<List<object>>()
            {
                new() {query, tableResult.Ok ? "Success" : "Failed"}
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