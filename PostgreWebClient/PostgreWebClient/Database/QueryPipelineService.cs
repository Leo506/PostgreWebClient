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
        var paginationResult = _paginationService.Paginate(viewModel.QueryModel.QueryText, viewModel.PaginationModel, connection);
        if (!paginationResult.Ok)
        {
            return new QueryViewModel()
            {
                ErrorModel = new ErrorModel()
                {
                    ErrorText = paginationResult.Exception?.Message ?? string.Empty
                }
            };
        }

        var queryResult = _commandService.ExecuteCommand(paginationResult.Result!, (connection as NpgsqlConnection)!);
        if (!queryResult.Ok)
        {
            return new QueryViewModel()
            {
                ErrorModel = new ErrorModel()
                {
                    ErrorText = queryResult.Exception?.Message ?? string.Empty
                }
            };
        }

        var infoResult = _databaseInfoService.GetDatabaseInfo((connection as NpgsqlConnection)!);
        if (!infoResult.Ok)
        {
            return new QueryViewModel()
            {
                ErrorModel = new ErrorModel()
                {
                    ErrorText = infoResult.Exception?.Message ?? string.Empty
                }
            };
        }
        var result = new QueryViewModel()
        {
            QueryModel = new QueryModel()
            {
                QueryResultTable = queryResult.Result
            },

            DatabaseInfoModel = infoResult.Result ?? new DatabaseInfo()
        };
        
        return result;
    }
}