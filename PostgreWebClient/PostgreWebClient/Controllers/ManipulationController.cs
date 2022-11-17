using Microsoft.AspNetCore.Mvc;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;
using PostgreWebClient.ViewModels;

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
        var sessionId = Request?.Cookies["session_id"];
        if (sessionId is null || !_connectionService.Connections.ContainsKey(sessionId))
            return Redirect("/Connection");
        return View();
    }

    [HttpGet]
    public ActionResult CloseConnection()
    {
        if (HttpContext.Request.Cookies.ContainsKey("session_id"))
            HttpContext.Response?.Cookies.Delete("session_id");
        return Redirect("/Connection");
    }
}