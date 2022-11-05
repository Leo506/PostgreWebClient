using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Controllers;
using PostgreWebClient.Database;
using PostgreWebClient.Models;

namespace PostgreWebClient.UnitTests;

public class ManipulationTests
{
    [Fact]
    public void Index_NoSessionId_Returns_Redirect()
    {
        // arrange
        var connectionServiceMock = new Mock<IConnectionService>();
        connectionServiceMock.Setup(service => service.Connections).Returns(new Dictionary<string, NpgsqlConnection>());
        var sut = new ManipulationController(connectionServiceMock.Object, null, null);

        // act
        var response = sut.Index();
        var result = response as RedirectResult;
        
        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void ExecuteCommand_NoSessionId_Returns_Redirect()
    {
        // arrange
        var connectionServiceMock = new Mock<IConnectionService>();
        connectionServiceMock.Setup(service => service.Connections).Returns(new Dictionary<string, NpgsqlConnection>());

        var sut = new ManipulationController(connectionServiceMock.Object, null, null);

        // act
        var response = sut.ExecuteCommand(default!);
        var result = response as RedirectResult;

        // assert
        result.Should().NotBeNull();
    }

    /*[Fact]
    public void ExecuteCommand_SessionIdExists_CommandService_Invoke()
    {
        // arrange
        var connectionServiceMock = new Mock<IConnectionService>();
        connectionServiceMock.Setup(service => service.Connections).Returns(new Dictionary<string, NpgsqlConnection>()
        {
            ["testId"] = null
        });

        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Request.Cookies["session_id"]).Returns("testId");

        var cmdServiceMock = new Mock<ICommandService>();
        cmdServiceMock.Setup(service => service.ExecuteCommand(It.IsAny<QueryModel>(), It.IsAny<NpgsqlConnection>()))
            .Returns(new QueryModel());

        var sut = new ManipulationController(connectionServiceMock.Object, cmdServiceMock.Object,
            new Mock<IDatabaseInfoService>().Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            }
        };

        // act
        sut.ExecuteCommand(default!);


        // assert
        cmdServiceMock.Invocations.Count.Should().Be(1);
    }*/
}