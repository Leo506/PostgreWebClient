using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Database;
using PostgreWebClient.Models;
using PostgreWebClient.UnitTests.FixtureAttributes;
using PostgreWebClient.ViewModels;

namespace PostgreWebClient.UnitTests.QueryPipelineTests;

public partial class QueryPipelineTests
{
    [Theory, AutoMoqData]
    public void HandleQuery_AllGood_ReturnsQueryViewModel(QueryPipelineService sut)
    {
        // act
        var result = sut.HandleQuery(MakeViewModel(), default!);

        // assert
        result.Should().NotBeNull();
    }

    [Theory, AutoMoqData]
    public void HandleQuery_AllGood_PaginationServiceInvoke([Frozen] Mock<IPaginationService> pagination,
        QueryPipelineService sut)
    {
        // act
        sut.HandleQuery(MakeViewModel(), default!);

        // assert
        pagination.Invocations.Count.Should().Be(1);
    }

    [Theory, AutoMoqData]
    public void HandleQuery_AllGood_CommandServiceInvoke([Frozen] Mock<ICommandService> command,
        QueryPipelineService sut)
    {
        // act
        sut.HandleQuery(MakeViewModel(), default!);

        // assert
        command.Invocations.Count.Should().Be(1);
    }

    [Theory, AutoMoqData]
    public void HandleQuery_AllGood_DatabaseInfoServiceInvoke([Frozen] Mock<IDatabaseInfoService> databaseInfo,
        QueryPipelineService sut)
    {
        // act
        sut.HandleQuery(MakeViewModel(), default!);

        // assert
        databaseInfo.Invocations.Count.Should().Be(1);
    }


    private QueryViewModel MakeViewModel()
    {
        return new QueryViewModel()
        {
            QueryModel = new QueryModel()
            {
                QueryText = "some query"
            }
        };
    }
}