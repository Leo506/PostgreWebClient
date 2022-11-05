using FluentAssertions;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.UnitTests.FixtureAttributes;

namespace PostgreWebClient.UnitTests.ConnectionService;

public partial class ConnectionServiceTests
{
    [Theory, AutoMoqData]
    public void Connect_TwoSameKeys_ReturnsResultNotOk(Database.ConnectionService sut)
    {
        // act
        sut.Connect("key", "connString");
        var result = sut.Connect("key", "connString");

        // assert
        result.Ok.Should().BeFalse();
    }
}