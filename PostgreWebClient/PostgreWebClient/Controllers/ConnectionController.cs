using Microsoft.AspNetCore.Mvc;
using PostgreWebClient.Abstractions;
using PostgreWebClient.ViewModel;

namespace PostgreWebClient.Controllers;

public class ConnectionController : Controller
{
    private IConnectionService _connectionService;

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
        try
        {
            _connectionService.Connect(viewModel.ToConnectionString());
            return Redirect("/home");
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}