using System.Net;
using Microsoft.AspNetCore.Mvc;
using PostgreWebClient.Abstractions;
using PostgreWebClient.ViewModel;

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
        var sessionId = Request?.Cookies["session_id"];
        if (sessionId is not null && _connectionService.Connections.ContainsKey(sessionId))
            return Redirect("/manipulation");
        return View(new ConnectionViewModel());
    }

    [HttpPost]
    public ActionResult Connect(ConnectionViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        try
        {
            var sessionId = Guid.NewGuid().ToString();
           AttachCookies("session_id", sessionId);
           _connectionService.Connect( sessionId, viewModel.ToConnectionString());
            return Redirect("/manipulation");
        }
        catch (Exception e)
        {
            return BadRequest();
        }
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