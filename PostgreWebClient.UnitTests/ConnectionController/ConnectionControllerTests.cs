using System.Net;
using AutoFixture.Xunit2;
using Calabonga.OperationResults;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;
using PostgreWebClient.UnitTests.FixtureAttributes;

namespace PostgreWebClient.UnitTests.ConnectionController;

public partial class ConnectionControllerTests
{
    [Theory, AutoMoqData]
    public void Connect_AllGood_Returns_Redirect([Greedy]Controllers.ConnectionController sut)
    {
        // act
        var response = sut.Connect(MakeConnection());
        var result = response as RedirectResult;

        // assert
        result.Should().NotBeNull();
    }

    
    [Theory, AutoMoqData]
    public void Connect_ConnectionServiceThrows_ReturnsBadRequest([Frozen] Mock<IConnectionService> connectionService,
        [Greedy] Controllers.ConnectionController sut)
    {
        // arrange
        connectionService.Setup(service => service.Connect(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new OperationResult<bool>()
            {
                Exception = new Exception()
            });
        
        // act
        var response = sut.Connect(new ConnectionModel());
        var result = (response as BadRequestResult)!.StatusCode;

        // assert
        result.Should().Be((int)HttpStatusCode.BadRequest);
    }
    
    
    [Theory, AutoMoqData]
    public void Connect_ModelInvalid_Returns_BadRequest([Greedy] Controllers.ConnectionController sut)
    {
        // arrange
        sut.ViewData.ModelState.AddModelError("error", "error");
        
        // act
        var response = sut.Connect(MakeConnection());
        var result = (response as BadRequestResult)!.StatusCode;

        // assert
        result.Should().Be((int)HttpStatusCode.BadRequest);

    }
    
    
    [Theory, AutoMoqData]
    public void Index_ExistsSessionId_ReturnsRedirect([Frozen] Mock<IConnectionService> connService,
        [Frozen] Mock<HttpContext> context, [Greedy] Controllers.ConnectionController sut)
    {
        // arrange
        connService.SetupGet(service => service.Connections).Returns(
            new Dictionary<string, NpgsqlConnection>()
            {
                ["guid"] = null
            });
        
        context.Setup(c => c.Request.Cookies["session_id"]).Returns("guid");

        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = context.Object
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