using System.Data;
using Calabonga.OperationResults;
using PostgreWebClient.Models;

namespace PostgreWebClient.Abstractions;

public interface IPaginationService
{
    OperationResult<string> Paginate(string query, PaginationModel paginationModel, IDbConnection connection);
}