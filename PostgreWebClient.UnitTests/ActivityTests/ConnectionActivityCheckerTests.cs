using System.Data;
using FluentAssertions;
using Moq;
using PostgreWebClient.ActivityCheck;
using PostgreWebClient.Models;

namespace PostgreWebClient.UnitTests.ActivityTests;

public class ConnectionActivityCheckerTests
{
    [Fact]
    public void Check_LastActivityNotExpire_NoActions()
    {
        // arrange
        var sut = new ConnectionActivityChecker(new ActivityCheckSettings()
        {
            TimeBeforeClose = TimeSpan.FromHours(1)
        });

        var collection = new ConnectionCollection()
        {
            ["id"] = new DbConnectionModel(new Mock<IDbConnection>().Object)
        };
        
        // act
        sut.Check(collection);

        // assert
        collection.Count.Should().Be(1);
    }


    [Fact]
    public void Check_ConnectionOpenTimeExpires_RemoveConnection()
    {
        // arrange
        var sut = new ConnectionActivityChecker(new ActivityCheckSettings()
        {
            TimeBeforeClose = TimeSpan.FromHours(1)
        });

        var collection = new ConnectionCollection()
        {
            ["id"] = new DbConnectionModelMock(new Mock<IDbConnection>().Object,
                DateTime.UtcNow.Subtract(TimeSpan.FromHours(2)))
        };

        // act
        sut.Check(collection);

        // assert
        collection.Should().BeEmpty();
    }
}