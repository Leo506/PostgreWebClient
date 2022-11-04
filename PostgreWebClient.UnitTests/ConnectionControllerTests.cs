using System.Net;
using Calabonga.OperationResults;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Controllers;
using PostgreWebClient.Models;

namespace PostgreWebClient.UnitTests;

public partial class ConnectionControllerTests
{
    [Fact]
    public void Connect_AllGood_Returns_Redirect()
    {
        // arrange
        var sut = new ConnectionController(MakeConnectionService(false).Object);

        // act
        var response = sut.Connect(MakeConnection());
        var result = response as RedirectResult;

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void Connect_ConnectionServiceThrows_Returns_BadRequest()
    {
        // arrange
        var sut = new ConnectionController(MakeConnectionService(true).Object);

        // act
        var response = sut.Connect(new ConnectionModel());
        var result = (response as BadRequestResult)!.StatusCode;

        // assert
        result.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public void Connect_ModelInvalid_Returns_BadRequest()
    {
        // arrange
        var connectionServiceMock = new Mock<IConnectionService>();
        var sut = new ConnectionController(connectionServiceMock.Object);
        sut.ViewData.ModelState.AddModelError("error", "error");
        
        // act
        var response = sut.Connect(MakeConnection());
        var result = (response as BadRequestResult)!.StatusCode;

        // assert
        result.Should().Be((int)HttpStatusCode.BadRequest);

    }

    [Fact]
    public void Index_ExistsSessionId_Returns_Redirect()
    {
        // arrange
        var connectionServiceMock = new Mock<IConnectionService>();
        connectionServiceMock.SetupGet(service => service.Connections).Returns(
            new Dictionary<string, NpgsqlConnection>()
            {
                ["guid"] = null
            });
        
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Request.Cookies["session_id"]).Returns("guid");

        var sut = new ConnectionController(connectionServiceMock.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            }
        };

        // act
        var response = sut.Index();
        var result = response as RedirectResult;

        // assert
        result.Should().NotBeNull();
    }

    private static ConnectionModel MakeConnection()
    {
        return new ConnectionModel()
        {
            UserId = "admin",
            Password = "password",
            Database = "Test",
            Host = "localhost",
            Port = 5432
        };
    }
}