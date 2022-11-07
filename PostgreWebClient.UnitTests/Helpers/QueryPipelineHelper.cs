using System.Data;
using Calabonga.OperationResults;
using Moq;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.UnitTests.Helpers;

public static class QueryPipelineHelper
{
    public static void MakeDefaultPagination(Mock<IPaginationService> pagination)
    {
        pagination.Setup(p => p.Paginate(It.IsAny<string>(), It.IsAny<PaginationModel>(), It.IsAny<IDbConnection>()))
            .Returns(new OperationResult<string>() { Result = "result" });
    }

    public static void MakeDefaultCommand(Mock<ICommandService> command)
    {
        command.Setup(c => c.ExecuteCommand(It.IsAny<string>(), It.IsAny<NpgsqlConnection>()))
            .Returns(new OperationResult<Table>()
            {
                Result = new Table()
            });
    }

    public static void MakeDefaultDatabaseInfo(Mock<IDatabaseInfoService> databaseInfo)
    {
        databaseInfo.Setup(service => service.GetDatabaseInfo(It.IsAny<NpgsqlConnection>()))
            .Returns(new OperationResult<DatabaseInfo>()
            {
                Result = new DatabaseInfo()
            });
    }
}