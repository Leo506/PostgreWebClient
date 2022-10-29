using Microsoft.AspNetCore.Mvc;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Controllers;

public class ManipulationController : Controller
{
    private readonly IConnectionService _connectionService;
    private readonly ICommandService _commandService;
    
    // GET
    public ManipulationController(IConnectionService connectionService, ICommandService commandService)
    {
        _connectionService = connectionService;
        _commandService = commandService;
    }

    public ActionResult Index()
    {
        var sessionId = Request?.Cookies["session_id"];
        if (sessionId is null || !_connectionService.Connections.ContainsKey(sessionId))
            return Redirect("/Connection");
        return View(new QueryModel());
    }


    [HttpPost]
    [ActionName("Index")]
    public ActionResult ExecuteCommand(QueryModel query)
    {
        var sessionId = Request?.Cookies["session_id"];
        if (sessionId is null || !_connectionService.Connections.ContainsKey(sessionId))
            return Redirect("/Connection");

        var result = _commandService.ExecuteCommand(query, _connectionService.Connections[sessionId]);
        
        return View("Index", result);
    }
}