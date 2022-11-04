using FluentAssertions;
using Moq;
using PostgreWebClient.Abstractions;

namespace PostgreWebClient.UnitTests.ConnectionService;

public partial class ConnectionServiceTests
{
    [Fact]
    public void Connect_TwoSameKeys_ReturnsResultNotOk()
    {
        // arrange
        var sut = new Database.ConnectionService(new Mock<IConnectionMaker>().Object);

        // act
        sut.Connect("key", "connString");
        var result = sut.Connect("key", "connString");

        // assert
        result.Ok.Should().BeFalse();
    }
}