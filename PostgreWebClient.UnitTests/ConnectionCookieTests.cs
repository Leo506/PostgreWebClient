using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Controllers;

namespace PostgreWebClient.UnitTests;

public partial class ConnectionTests
{
    [Fact]
    public void Connect_GenerateSessionId_Success()
    {
        // arrange
        var connectionServiceMock = new Mock<IConnectionService>();
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context =>
                context.Response.Cookies.Append("session_id", It.IsAny<string>(), It.IsAny<CookieOptions>()))
            .Verifiable();

        var sut = new ConnectionController(connectionServiceMock.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            }
        };
        // act
        sut.Connect(MakeConnection());


        // assert
        contextMock.Verify();
    }

    [Fact]
    public void Connect_SetCookieExpireTime_Success()
    {
        // arrange
        var connectionServiceMock = new Mock<IConnectionService>();
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Response.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(),
                It.Is<CookieOptions>(options =>
                    options.Expires - DateTimeOffset.UtcNow + TimeSpan.FromSeconds(30) >= TimeSpan.FromHours(1))))
            .Verifiable();

        var sut = new ConnectionController(connectionServiceMock.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object
            }
        };

        // act
        sut.Connect(MakeConnection());

        // assert
        contextMock.Verify();
    }
}