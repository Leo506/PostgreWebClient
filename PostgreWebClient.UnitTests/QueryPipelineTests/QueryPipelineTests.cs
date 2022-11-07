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
    public void HandleQuery_PaginationServiceInvoke_ChangeQueryText([Frozen] Mock<IPaginationService> pagination,
        [Frozen] Mock<ICommandService> command, QueryPipelineService sut)
    {
        // arrange
        pagination.Setup(p => p.Paginate(It.IsAny<string>(), It.IsAny<PaginationModel>(), It.IsAny<IDbConnection>()))
            .Returns(new OperationResult<string>() { Result = "changed query" });
        command.Setup(c => c.ExecuteCommand("changed query", It.IsAny<NpgsqlConnection>())).Verifiable();

        // act
        sut.HandleQuery(MakeViewModel(), default!);

        // assert
        command.Verify();
    }

    [Theory, AutoMoqData]
    public void HandleQuery_CommandServiceInvoke_SetTableInResult([Frozen] Mock<IPaginationService> pagination,
        [Frozen] Mock<ICommandService> command, [Frozen] Mock<IDatabaseInfoService> databaseInfo,
        QueryPipelineService sut)
    {
        // arrange
        QueryPipelineHelper.MakeDefaultPagination(pagination);
        QueryPipelineHelper.MakeDefaultDatabaseInfo(databaseInfo);

        command.Setup(c => c.ExecuteCommand(It.IsAny<string>(), It.IsAny<NpgsqlConnection>()))
            .Returns(new OperationResult<Table>()
            {
                Result = new Table()
                {
                    Columns = new List<string>() { "Column" },
                    Rows = new List<List<object>>()
                    {
                        new() { "Row" }
                    }
                }
            });

        // act
        var result = sut.HandleQuery(MakeViewModel(), default!);

        // assert
        result.QueryModel.QueryResultTable!.Columns.Should().Contain("Column");
        result.QueryModel.QueryResultTable!.Rows![0][0].Should().Be("Row");
    }


    [Theory, AutoMoqData]
    public void HandleQuery_DatabaseInfoServiceInvoke_SetDatabaseInfoInResult(
        [Frozen] Mock<IPaginationService> pagination,
        [Frozen] Mock<ICommandService> command, [Frozen] Mock<IDatabaseInfoService> databaseInfo,
        QueryPipelineService sut)
    {
        // arrange
        QueryPipelineHelper.MakeDefaultPagination(pagination);
        QueryPipelineHelper.MakeDefaultCommand(command);

        databaseInfo.Setup(service => service.GetDatabaseInfo(It.IsAny<NpgsqlConnection>())).Returns(
            new OperationResult<DatabaseInfo>()
            {
                Result = new DatabaseInfo()
                {
                    Schemas = new List<SchemaModel>()
                    {
                        new SchemaModel()
                        {
                            SchemaName = "Schema",
                            Tables = new List<string>() { "Table" }
                        }
                    }
                }
            });


        // act
        var result = sut.HandleQuery(MakeViewModel(), default!);

        // assert
        result.DatabaseInfoModel.Schemas.Count.Should().Be(1);
        result.DatabaseInfoModel.Schemas[0].SchemaName.Should().Be("Schema");
        result.DatabaseInfoModel.Schemas[0].Tables.Should().Contain("Table");
    }
}