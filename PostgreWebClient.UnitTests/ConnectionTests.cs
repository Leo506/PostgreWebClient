using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Controllers;
using PostgreWebClient.ViewModel;

namespace PostgreWebClient.UnitTests;

public class ConnectionTests
{
    [Fact]
    public void Connect_AllGood_Returns_Redirect()
    {
        // arrange
        var sut = new ConnectionController(new Mock<IConnectionService>().Object);

        // act
        var response = sut.Connect(new ConnectionViewModel()
        {
            UserId = "admin",
            Password = "password",
            Database = "Test",
            Host = "localhost",
            Port = 5432
        });
        var result = response as RedirectResult;

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void Connect_IncorrectConnectionString_Returns_BadRequest()
    {
        // arrange
        var connectionServiceMock = new Mock<IConnectionService>();
        connectionServiceMock.Setup(service => service.Connect(It.IsAny<string>())).Throws(new Exception());
        var sut = new ConnectionController(connectionServiceMock.Object);

        // act
        var response = sut.Connect(new ConnectionViewModel());
        var result = (response as BadRequestResult)!.StatusCode;

        // assert
        result.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public void Connect_GenerateSessionId_Success()
    {
        // arrange
        var connectionServiceMock = new Mock<IConnectionService>();
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Response.Cookies.Append("session_id", It.IsAny<string>())).Verifiable();

        var sut = new ConnectionController(connectionServiceMock.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            }
        };
        // act
        sut.Connect(new ConnectionViewModel()
        {
            UserId = "admin",
            Password = "password",
            Database = "Test",
            Host = "localhost",
            Port = 5432
        });


        // assert
        contextMock.Verify();
    }
}