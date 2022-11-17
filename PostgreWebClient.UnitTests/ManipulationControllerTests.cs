using System.Data;
using System.Net;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
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
    public void Index_SessionIdExists_ReturnsView([Frozen] Mock<IConnectionService> connectionService,
        [Greedy] ManipulationController sut)
    {
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Request.Cookies["session_id"]).Returns("session_id");

        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = contextMock.Object
        };

        connectionService.SetupGet(service => service.Connections).Returns(new Dictionary<string, NpgsqlConnection>()
        {
            ["session_id"] = default!
        });

        // act
        var response = sut.Index();
        var result = response as ViewResult;

        // assert
        result.Should().NotBeNull();
    }

    [Theory, AutoMoqData]
    public void CloseConnection_AllGood_ReturnsRedirect([Greedy] ManipulationController sut)
    {
        // arrange
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Request.Cookies.ContainsKey("session_id")).Returns(true);
        
        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = contextMock.Object
        };
        
        // act
        var response = sut.CloseConnection();
        var result = response as RedirectResult;
        
        // assert
        result.Should().NotBeNull();
    }

    [Theory, AutoMoqData]
    public void CloseConnection_ThereAreCookies_RemoveCookie([Greedy] ManipulationController sut)
    {
        // arrange
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Request.Cookies.ContainsKey("session_id")).Returns(true);
        contextMock.Setup(context => context.Response.Cookies.Delete("session_id")).Verifiable();
        contextMock.Name = "test";

        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = contextMock.Object
        };
        
        // act
        sut.CloseConnection();
        
        // assert
        contextMock.Verify();
    }

    [Theory, AutoMoqData]
    public void CloseConnection_NoCookies_RemoveCookieNotInvoke([Greedy] ManipulationController sut)
    {
        // arrange
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Request.Cookies.ContainsKey("session_id")).Returns(false);
        

        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = contextMock.Object
        };
        
        // act
        sut.CloseConnection();
        
        // assert
        contextMock.Verify(context => context.Response.Cookies.Delete(It.IsAny<string>()), Times.Exactly(0));
    }

    [Theory, AutoMoqData]
    public void CloseConnection_ThereAreCookies_RemoveConnection([Frozen] Mock<IConnectionService> connection,
        [Greedy] ManipulationController sut)
    {
        // arrange
        var connDict = new Dictionary<string, NpgsqlConnection>()
        {
            ["id"] = default!
        };

        connection.SetupGet(service => service.Connections).Returns(connDict);
        
        var contextMock = new Mock<HttpContext>();
        contextMock.Setup(context => context.Request.Cookies.ContainsKey("session_id")).Returns(true);
        contextMock.SetupGet(context => context.Request.Cookies["session_id"]).Returns("id");
        
        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = contextMock.Object
        };
        
        // act
        sut.CloseConnection();
        
        // assert
        connDict.Count.Should().Be(0);
    }
}