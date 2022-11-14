using Microsoft.AspNetCore.Mvc;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;
using PostgreWebClient.ViewModels;

namespace PostgreWebClient.Controllers;

public class ManipulationController : Controller
{
    private readonly IConnectionService _connectionService;
    private readonly IQueryPipeline _queryPipeline;
    
    // GET
    public ManipulationController(IConnectionService connectionService, IQueryPipeline queryPipeline)
    {
        _connectionService = connectionService;
        _queryPipeline = queryPipeline;
    }

    public ActionResult Index()
    {
        var sessionId = Request?.Cookies["session_id"];
        if (sessionId is null || !_connectionService.Connections.ContainsKey(sessionId))
            return Redirect("/Connection");
        var viewModel = _queryPipeline.HandleQuery(new QueryViewModel()
        {
            QueryModel = new QueryModel()
            {
                QueryText = "SELECT 1"
            },
            PaginationModel = new PaginationModel()
            {
                CurrentPage = 1,
                TotalRecordsCount = 1
            }
        }, _connectionService.Connections[sessionId]);
        return View(viewModel);
    }


    [HttpPost]
    [ActionName("Index")]
    public ActionResult ExecuteCommand(QueryViewModel query)
    {
        var sessionId = Request?.Cookies["session_id"];
        if (sessionId is null || !_connectionService.Connections.ContainsKey(sessionId))
            return Redirect("/Connection");

        //_commandService.ExecuteCommand(query.QueryText, _connectionService.Connections[sessionId]);
        //result.DatabaseInfo = _databaseInfoService.GetDatabaseInfo(_connectionService.Connections[sessionId]);

        var viewModel = _queryPipeline.HandleQuery(query, _connectionService.Connections[sessionId]);

        return View("Index", viewModel);
    }
}