using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Controllers;
using PostgreWebClient.Models;
using PostgreWebClient.UnitTests.FixtureAttributes;
using PostgreWebClient.ViewModels;

namespace PostgreWebClient.UnitTests;

public class ManipulationControllerTests
{
    [Theory, AutoMoqData]
    public void Index_NoSessionId_ReturnsRedirect([Greedy] ManipulationController sut)
    {
        // act
        var response = sut.Index();
        var result = response as RedirectResult;
        
        // assert
        result.Should().NotBeNull();
    }

    [Theory, AutoMoqData]
    public void ExecuteCommand_NoSessionId_ReturnsRedirect([Greedy] ManipulationController sut)
    {
        // act
        var response = sut.ExecuteCommand(default!);
        var result = response as RedirectResult;

        // assert
        result.Should().NotBeNull();
    }

    [Theory, AutoMoqData]
    public void ExecuteCommand_SessionIdExists_QueryPipelineServiceInvoke([Frozen] Mock<IConnectionService> connService,
        [Frozen] Mock<IQueryPipeline> queryPipeline, [Greedy] ManipulationController sut)
    {
        // arrange
        connService.Setup(service => service.Connections).Returns(new Dictionary<string, NpgsqlConnection>()
        {
            ["testId"] = null
        });

        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Request.Cookies["session_id"]).Returns("testId");

        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = contextMock.Object
        };

        // act
        sut.ExecuteCommand(new QueryViewModel());


        // assert
        queryPipeline.Invocations.Count.Should().Be(1);
    }
}