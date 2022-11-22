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
    private readonly IPaginationService _paginationService;

    public ManipulationHub(IDatabaseInfoService databaseInfoService, IConnectionService connectionService,
        ICommandService commandService, IPaginationService paginationService)
    {
        _databaseInfoService = databaseInfoService;
        _connectionService = connectionService;
        _commandService = commandService;
        _paginationService = paginationService;
    }

    public async Task ExecuteQuery(string query, string sessionId, PaginationModel pagination)
    {
        var connectionModel = _connectionService.Connections[sessionId] as DbConnectionModel;
        var paginationResult = _paginationService.Paginate(query, pagination, connectionModel!.Connection);
        var table = _commandService.ExecuteCommand(paginationResult.Result!,
            (connectionModel!.Connection as NpgsqlConnection)!);
        
        await Clients.Caller.SendAsync("getTable", table, pagination);
    }

    public async Task GetDatabaseInfo(string sessionId)
    {
        var connectionModel = _connectionService.Connections[sessionId] as DbConnectionModel;
        var databaseInfo = _databaseInfoService.GetDatabaseInfo((connectionModel!.Connection as NpgsqlConnection)!);
        await Clients.Caller.SendAsync("getDatabaseInfo", databaseInfo);
    }
}