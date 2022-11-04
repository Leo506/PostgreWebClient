using System.Data;
using FluentAssertions;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Database;

namespace PostgreWebClient.UnitTests;

public class ConnectionServiceTests
{
    [Fact]
    public void Connect_AllGood_AddConnection()
    {
        // arrange
        var makerMock = new Mock<IConnectionMaker>();
        makerMock.Setup(maker => maker.MakeConnection(It.IsAny<string>())).Returns(default(IDbConnection)!);
        var sut = new ConnectionService(makerMock.Object);

        // act
        sut.Connect("key", "validConnString");

        // assert
        (sut as IConnectionService).Connections.Count.Should().Be(1);
    }

    [Fact]
    public void Connect_MakeConnectionThrows_ReturnsResultNotOk()
    {
        // arrange
        var makerMock = new Mock<IConnectionMaker>();
        makerMock.Setup(maker => maker.MakeConnection(It.IsAny<string>())).Throws(new Exception("error"));

        var sut = new ConnectionService(makerMock.Object);

        // act
        var result = sut.Connect("key", "connString");

        // assert
        result.Ok.Should().BeFalse();
    }
}