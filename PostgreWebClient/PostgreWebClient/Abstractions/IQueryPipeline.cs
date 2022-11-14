using System.Data;
using PostgreWebClient.ViewModels;

namespace PostgreWebClient.Abstractions;

public interface IQueryPipeline
{
    QueryViewModel HandleQuery(QueryViewModel viewModel, IDbConnection connection);
}