using Microsoft.AspNetCore.Mvc;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Controllers;

public class ManipulationController : Controller
{
    private readonly IConnectionService _connectionService;
    private readonly ICommandService _commandService;
    private readonly IDatabaseInfoService _databaseInfoService;
    
    // GET
    public ManipulationController(IConnectionService connectionService, ICommandService commandService,
        IDatabaseInfoService databaseInfoService)
    {
        _connectionService = connectionService;
        _commandService = commandService;
        _databaseInfoService = databaseInfoService;
    }

    public ActionResult Index()
    {
        var sessionId = Request?.Cookies["session_id"];
        if (sessionId is null || !_connectionService.Connections.ContainsKey(sessionId))
            return Redirect("/Connection");
        var model = new QueryModel();
        model.DatabaseInfo = _databaseInfoService.GetDatabaseInfo(_connectionService.Connections[sessionId]);
        model.Pagination = new PaginationModel()
        {
            CurrentPage = 1,
            TotalRecordsCount = 0
        };
        return View(model);
    }


    [HttpPost]
    [ActionName("Index")]
    public ActionResult ExecuteCommand(QueryModel query)
    {
        var sessionId = Request?.Cookies["session_id"];
        if (sessionId is null || !_connectionService.Connections.ContainsKey(sessionId))
            return Redirect("/Connection");

        var result = _commandService.ExecuteCommand(query, _connectionService.Connections[sessionId]);
        result.DatabaseInfo = _databaseInfoService.GetDatabaseInfo(_connectionService.Connections[sessionId]);
        
        return View("Index", result);
    }
}