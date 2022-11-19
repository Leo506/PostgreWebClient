using System.Data;
using FluentAssertions;
using Moq;
using PostgreWebClient.Models;

namespace PostgreWebClient.UnitTests;

public class ConnectionCollectionTests
{
    [Fact]
    public void ConnectionCollection_SuccessfulCreated()
    {
        // act
        var result = new ConnectionCollection();

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void ConnectionCollection_CollectionInitializing_Success()
    {
        // act
        var result = new ConnectionCollection()
        {
            ["id1"] = new Mock<IDbConnection>().Object,
            ["id2"] = new Mock<IDbConnection>().Object
        };

        // assert
        result.Count.Should().Be(2);
    }

    [Fact]
    public void CollectionInitializing_AllGood_InvokeConnectionOpen()
    {
        // arrange
        var connectionMock = new Mock<IDbConnection>();

        // act
        var sut = new ConnectionCollection()
        {
            ["id"] = connectionMock.Object
        };

        // assert
        connectionMock.Verify(connection => connection.Open(), Times.Exactly(1));
    }

    [Fact]
    public void Add_AllGood_ChangeCount()
    {
        // arrange
        var sut = new ConnectionCollection();
        var startCount = sut.Count;

        // act
        sut.Add("id", new Mock<IDbConnection>().Object);
        var resultCount = sut.Count;

        var result = resultCount - startCount;
        
        // assert
        result.Should().Be(1);
    }

    [Fact]
    public void Add_AllGood_InvokeConnectionOpen()
    {
        // arrange
        var connectionMock = new Mock<IDbConnection>();

        var sut = new ConnectionCollection();
        
        // act
        sut.Add("id", connectionMock.Object);

        // assert
        connectionMock.Verify(connection => connection.Open(), Times.Exactly(1));
    }

    [Fact]
    public void RemoveById_AllGood_ChangeCount()
    {
        // arrange
        var sut = new ConnectionCollection()
        {
            ["id"] = new Mock<IDbConnection>().Object
        };

        var startCount = sut.Count;

        // act
        sut.Remove("id");

        var resultCount = sut.Count;

        // assert
        resultCount.Should().Be(0);
        (startCount - resultCount).Should().Be(1);
    }

    [Fact]
    public void RemoveById_AllGood_CloseConnection()
    {
        // arrange
        var connectionMock = new Mock<IDbConnection>();
        var sut = new ConnectionCollection()
        {
            ["id"] = connectionMock.Object
        };

        // act
        sut.Remove("id");

        // assert
        connectionMock.Verify(connection => connection.Close(), Times.Exactly(1));
    }
}