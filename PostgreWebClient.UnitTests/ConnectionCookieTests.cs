using Calabonga.OperationResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Controllers;

namespace PostgreWebClient.UnitTests;

public partial class ConnectionControllerTests
{
    [Fact]
    public void Connect_GenerateSessionId_Success()
    {
        // arrange
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context =>
                context.Response.Cookies.Append("session_id", It.IsAny<string>(), It.IsAny<CookieOptions>()))
            .Verifiable();

        var sut = new ConnectionController(MakeConnectionService(false).Object)
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
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Response.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(),
                It.Is<CookieOptions>(options =>
                    options.Expires - DateTimeOffset.UtcNow + TimeSpan.FromSeconds(30) >= TimeSpan.FromHours(1))))
            .Verifiable();

        var sut = new ConnectionController(MakeConnectionService(false).Object)
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

    private Mock<IConnectionService> MakeConnectionService(bool hasError)
    {
        var mock = new Mock<IConnectionService>();
        mock.Setup(service => service.Connect(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(hasError
                ? new OperationResult<bool>()
                {
                    Exception = new Exception("error")
                }
                : OperationResult.CreateResult<bool>());

        return mock;
    }
}