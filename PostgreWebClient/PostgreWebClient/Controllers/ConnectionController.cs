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
    public IActionResult Index()
    {
        return View(new ConnectionViewModel());
    }

    [HttpPost]
    public ActionResult Connect(ConnectionViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        if (Request?.Cookies["session_id"] is not null)
            return Redirect("/home");
        
        try
        {
            var sessionId = Guid.NewGuid().ToString();
            Response?.Cookies.Append("session_id", sessionId);
            _connectionService.Connect( sessionId, viewModel.ToConnectionString());
            return Redirect("/home");
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}