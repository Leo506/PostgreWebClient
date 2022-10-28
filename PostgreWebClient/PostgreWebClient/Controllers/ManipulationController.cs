using Microsoft.AspNetCore.Mvc;

namespace PostgreWebClient.Controllers;

public class ManipulationController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}