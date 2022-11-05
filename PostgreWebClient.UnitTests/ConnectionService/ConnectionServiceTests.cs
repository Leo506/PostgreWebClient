using System.Data;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.UnitTests.FixtureAttributes;

namespace PostgreWebClient.UnitTests.ConnectionService;

public partial class ConnectionServiceTests
{
    [Theory, AutoMoqData]
    public void Connect_AllGood_AddConnection(Database.ConnectionService sut)
    {
        // act
        sut.Connect("key", "validConnString");

        // assert
        (sut as IConnectionService).Connections.Count.Should().Be(1);
    }

    [Theory, AutoMoqData]
    public void Connect_MakeConnectionThrows_ReturnsResultNotOk([Frozen] Mock<IConnectionMaker> maker, 
        Database.ConnectionService sut)
    {
        // arrange
        maker.Setup(connectionMaker => connectionMaker.MakeConnection(It.IsAny<string>())).Throws(new Exception());

        // act
        var result = sut.Connect("key", "connString");

        // assert
        result.Ok.Should().BeFalse();
    }
}