using System.Data;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Database;
using PostgreWebClient.Models;
using PostgreWebClient.UnitTests.FixtureAttributes;

namespace PostgreWebClient.UnitTests;

public class PaginationServiceTests
{
    private const string OriginalQuery = "SELECT * FROM TableName";
    
    [Theory, AutoMoqData]
    public void Paginate_AllGood_ChangeQueryText([Frozen] Mock<ICommandExecutor> executor, PaginationService sut)
    {
        // arrange
        MakeExecutor(executor);

        // act
        var newQuery = sut.Paginate(OriginalQuery, new PaginationModel(), default!);

        // assert
        newQuery.Result.Should().NotBe(OriginalQuery);
    }

    [Theory, AutoMoqData]
    public void Paginate_AllGood_UsePaginationModel([Frozen] Mock<ICommandExecutor> executor, PaginationService sut)
    {
        // arrange
        var expectedQuery =
            $"SELECT * FROM ({OriginalQuery}) as TmpTable OFFSET 0 LIMIT {PaginationModel.PageSize}";
        
        MakeExecutor(executor, new Table() { Rows = new List<List<object>>() { new() { (long)10 } } });

        // act
        var newQuery = sut.Paginate(OriginalQuery, new PaginationModel()
        {
            CurrentPage = 1,
            TotalCount = 10
        }, default!);

        // assert
        newQuery.Result.Should().Be(expectedQuery);
    }

    [Theory, AutoMoqData]
    public void Paginate_AllGood_SetTotalRecordsCountInPaginationModel([Frozen] Mock<ICommandExecutor> executor,
        PaginationService sut)
    {
        // arrange
        var paginationModel = new PaginationModel();

        MakeExecutor(executor, new Table() { Rows = new List<List<object>>() { new() { (long)100 } } });


        // act
        sut.Paginate(OriginalQuery, paginationModel, default!);

        // assert
        paginationModel.TotalCount.Should().Be(100);
    }

    [Theory, AutoMoqData]
    public void Paginate_ExecutorThrows_ReturnsResultNotOk([Frozen] Mock<ICommandExecutor> executor,
        PaginationService sut)
    {
        // arrange
        executor.Setup(commandExecutor => commandExecutor.Execute(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Throws(new Exception());

        // act
        var result = sut.Paginate(OriginalQuery, new PaginationModel(), default!);

        // assert
        result.Ok.Should().BeFalse();
    }

    [Theory, AutoMoqData]
    public void Paginate_NoSelectQuery_ReturnsOriginalQuery(PaginationService sut)
    {
        // arrange
        const string original = "CREATE TABLE TEST(id SERIAL PRIMARY KEY, name VARCHAR(50) NOT NULL )";
        
        // act
        var result = sut.Paginate(original, new PaginationModel(), default!);
        
        // assert
        result.Result.Should().Be(original);
    }
    private static void MakeExecutor(Mock<ICommandExecutor> executor, Table? table = null)
    {
        executor.Setup(commandExecutor => commandExecutor.Execute(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Returns(table ?? new Table()
            {
                Rows = new List<List<object>>()
                {
                    new() { 1 }
                }
            });

    }
}