using Microsoft.AspNetCore.Mvc;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Controllers;

public class ManipulationController : Controller
{
    private readonly IConnectionService _connectionService;
    
    // GET
    public ManipulationController(IConnectionService connectionService)
    {
        _connectionService = connectionService;
    }

    public ActionResult Index()
    {
        /*var sessionId = Request?.Cookies["session_id"];
        if (sessionId is null || !_connectionService.Connections.ContainsKey(sessionId))
            return Redirect("/Connection");*/
        return View(new QueryModel());
    }

    [HttpPost]
    public ActionResult ExecuteCommand(QueryModel query)
    {
        return View("Index", query);
    }
}