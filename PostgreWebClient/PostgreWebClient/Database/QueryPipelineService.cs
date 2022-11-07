using System.Data;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;
using PostgreWebClient.ViewModels;

namespace PostgreWebClient.Database;

public class QueryPipelineService
{
    private readonly IPaginationService _paginationService;
    private readonly ICommandService _commandService;
    private readonly IDatabaseInfoService _databaseInfoService;

    public QueryPipelineService(IPaginationService paginationService, ICommandService commandService,
        IDatabaseInfoService databaseInfoService)
    {
        _paginationService = paginationService;
        _commandService = commandService;
        _databaseInfoService = databaseInfoService;
    }

    public QueryViewModel HandleQuery(QueryViewModel viewModel, IDbConnection connection)
    {
        var newQuery = _paginationService.Paginate(viewModel.QueryModel.QueryText, viewModel.PaginationModel, connection);

        var result = new QueryViewModel()
        {
            QueryModel = new QueryModel()
            {
                QueryResultTable = _commandService.ExecuteCommand(newQuery.Result!, (connection as NpgsqlConnection)!)
                    ?.Result
            },

            DatabaseInfoModel = _databaseInfoService.GetDatabaseInfo((connection as NpgsqlConnection)!).Result ??
                                new DatabaseInfo()
        };
        
        return result;
    }
}