using AutoFixture.Xunit2;
using Calabonga.OperationResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.UnitTests.FixtureAttributes;

namespace PostgreWebClient.UnitTests.ConnectionController;

public partial class ConnectionControllerTests
{
    [Theory, AutoMoqData]
    public void Connect_GenerateSessionId_Success([Greedy] Controllers.ConnectionController sut)
    {
        // arrange
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context =>
                context.Response.Cookies.Append("session_id", It.IsAny<string>(), It.IsAny<CookieOptions>()))
            .Verifiable();

        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = contextMock.Object
        };
        
        // act
        sut.Connect(MakeConnection());


        // assert
        contextMock.Verify();
    }

    [Theory, AutoMoqData]
    public void Connect_SetCookieExpireTime_Success([Greedy] Controllers.ConnectionController sut)
    {
        // arrange
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Response.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(),
                It.Is<CookieOptions>(options =>
                    options.Expires - DateTimeOffset.UtcNow + TimeSpan.FromSeconds(30) >= TimeSpan.FromHours(1))))
            .Verifiable();

        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = contextMock.Object
        };

        // act
        sut.Connect(MakeConnection());

        // assert
        contextMock.Verify();
    }
}