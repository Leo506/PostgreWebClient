using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Controllers;

namespace PostgreWebClient.UnitTests;

public class ManipulationTests
{
    [Fact]
    public void Index_NoSessionId_Returns_Redirect()
    {
        // arrange
        var connectionServiceMock = new Mock<IConnectionService>();
        connectionServiceMock.Setup(service => service.Connections).Returns(new Dictionary<string, NpgsqlConnection>());
        var sut = new ManipulationController(connectionServiceMock.Object);

        // act
        var response = sut.Index();
        var result = response as RedirectResult;
        
        // assert
        result.Should().NotBeNull();
    }
}