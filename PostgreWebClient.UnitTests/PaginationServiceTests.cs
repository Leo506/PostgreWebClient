using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Database;
using PostgreWebClient.Factories;
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
        newQuery.Should().NotBe(OriginalQuery);
    }

    [Theory, AutoMoqData]
    public void Paginate_AllGood_UsePaginationModel([Frozen] Mock<ICommandExecutor> executor)
    {
        // arrange
        var expectedQuery =
            $"SELECT * FROM ({OriginalQuery}) as TmpTable OFFSET 0 LIMIT {PaginationModel.PageSize}";
        
        MakeExecutor(executor, new Table() { Rows = new List<List<object>>() { new() { 10 } } });
        executor.Name = "some name";

        var sut = new PaginationService();

        
        // act
        var newQuery = sut.Paginate(OriginalQuery, new PaginationModel()
        {
            CurrentPage = 1,
            TotalRecordsCount = 10
        }, default!);

        // assert
        executor.Invocations.Count.Should().Be(1);
        newQuery.Should().Be(expectedQuery);
    }

    [Theory, AutoMoqData]
    public void Paginate_AllGood_SetTotalRecordsCountInPaginationModel([Frozen] Mock<ICommandExecutor> executor)
    {
        // arrange
        const string originalQuery = "SELECT * FROM TableName";
        var paginationModel = new PaginationModel();

        MakeExecutor(executor, new Table() { Rows = new List<List<object>>() { new() { 100 } } });

        var sut = new PaginationService();


        // act
        sut.Paginate(originalQuery, paginationModel, default!);

        // assert
        paginationModel.TotalRecordsCount.Should().Be(100);
    }

    private static void MakeExecutor(Mock<ICommandExecutor> executor, Table? table = null)
    {
        executor.Setup(commandExecutor => commandExecutor.Execute()).Returns(table ?? new Table()
        {
            Rows = new List<List<object>>()
            {
                new() { 1 }
            }
        });

        NpgsqlExecutorFactory.Executor = executor.Object;
    }
}