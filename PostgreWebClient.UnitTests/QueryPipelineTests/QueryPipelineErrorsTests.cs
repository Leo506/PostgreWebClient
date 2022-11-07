using System.Data;
using AutoFixture.Xunit2;
using Calabonga.OperationResults;
using FluentAssertions;
using Moq;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Database;
using PostgreWebClient.Models;
using PostgreWebClient.UnitTests.FixtureAttributes;
using PostgreWebClient.UnitTests.Helpers;

namespace PostgreWebClient.UnitTests.QueryPipelineTests;

public partial class QueryPipelineTests
{
    [Theory, AutoMoqData]
    public void HandleQuery_PaginationServiceThrows_ReturnsResultWithError([Frozen] Mock<IPaginationService> pagination,
        QueryPipelineService sut)
    {
        // arrange
        pagination.Setup(p => p.Paginate(It.IsAny<string>(), It.IsAny<PaginationModel>(), It.IsAny<IDbConnection>()))
            .Returns(new OperationResult<string>() { Exception = new Exception() });

        // act
        var result = sut.HandleQuery(MakeViewModel(), default!);

        // assert
        result.ErrorModel.Should().NotBeNull();
    }


    [Theory, AutoMoqData]
    public void HandleQuery_CommandServiceThrows_ReturnsResultWithError([Frozen] Mock<IPaginationService> pagination,
        [Frozen] Mock<ICommandService> cmd, QueryPipelineService sut)
    {
        // arrange
        QueryPipelineHelper.MakeDefaultPagination(pagination);

        cmd.Setup(service => service.ExecuteCommand(It.IsAny<string>(), It.IsAny<NpgsqlConnection>()))
            .Returns(new OperationResult<Table>() { Exception = new Exception() });

        // act
        var result = sut.HandleQuery(MakeViewModel(), default!);

        // assert
        result.ErrorModel.Should().NotBeNull();
    }

    [Theory, AutoMoqData]
    public void HandleQuery_DatabaseInfoServiceThrows_ReturnsResultWithError(
        [Frozen] Mock<IPaginationService> pagination,
        [Frozen] Mock<ICommandService> command, [Frozen] Mock<IDatabaseInfoService> databaseInfo,
        QueryPipelineService sut)
    {
        // arrange
        QueryPipelineHelper.MakeDefaultPagination(pagination);
        QueryPipelineHelper.MakeDefaultCommand(command);

        databaseInfo.Setup(service => service.GetDatabaseInfo(It.IsAny<NpgsqlConnection>()))
            .Returns(new OperationResult<DatabaseInfo>() { Exception = new Exception() });

        // act
        var result = sut.HandleQuery(MakeViewModel(), default!);

        // assert
        result.ErrorModel.Should().NotBeNull();
    }
}