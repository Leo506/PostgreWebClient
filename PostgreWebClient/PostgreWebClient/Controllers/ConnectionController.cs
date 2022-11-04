using System.Net;
using Microsoft.AspNetCore.Mvc;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Controllers;

public class ConnectionController : Controller
{
    private readonly IConnectionService _connectionService;

    public ConnectionController(IConnectionService connectionService)
    {
        _connectionService = connectionService;
    }

    // GET
    public ActionResult Index()
    {
        var sessionId = Request.Cookies["session_id"];
        if (sessionId is not null && _connectionService.Connections.ContainsKey(sessionId))
            return Redirect("/manipulation");
        
        return View(new ConnectionModel());
    }

    [HttpPost]
    public ActionResult Connect(ConnectionModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var sessionId = Guid.NewGuid().ToString();
        AttachCookies("session_id", sessionId);
        
        var result = _connectionService.Connect( sessionId, model.ToConnectionString());
        if (result.Ok)
            return Redirect("/manipulation");
        
        return BadRequest();
    }

    private void AttachCookies(string key, string value, DateTimeOffset? expires = null)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = expires ?? DateTimeOffset.Now.AddHours(1)
        };
        Response?.Cookies.Append(key, value, cookieOptions);
    }
}