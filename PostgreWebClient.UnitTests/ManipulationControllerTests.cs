using System.Net;
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
}