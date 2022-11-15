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
        var table = _commandService.ExecuteCommand(query, connection);
        
        await Clients.Caller.SendAsync("getTable", table);
    }

    public async Task GetDatabaseInfo(string sessionId)
    {
        var connection = _connectionService.Connections[sessionId];
        var databaseInfo = _databaseInfoService.GetDatabaseInfo(connection);
        await Clients.Caller.SendAsync("getDatabaseInfo", databaseInfo);
    }
}