using System.Data;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Database;
using PostgreWebClient.Models;
using PostgreWebClient.UnitTests.FixtureAttributes;

namespace PostgreWebClient.UnitTests;

public class DatabaseInfoServiceTests
{
    [Theory, AutoMoqData]
    public void GetDatabaseInfo_AllGood_ReturnsResultOk([Frozen] Mock<ICommandExecutor> executor, DatabaseInfoService sut)
    {
        // arrange
        executor.Setup(commandExecutor => commandExecutor.Execute(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Returns(new Table()
            {
                Columns = new List<string>(),
                Rows = new List<List<object>>()
            });
        
        
        // act
        var result = sut.GetDatabaseInfo(default!);

        // assert
        result.Ok.Should().BeTrue();
    }

    [Theory, AutoMoqData]
    public void GetDatabaseInfo_AllGood_ReturnsInfo([Frozen] Mock<ICommandExecutor> executor, DatabaseInfoService sut)
    {
        // arrange
        executor.Setup(commandExecutor => commandExecutor.Execute(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Returns(new Table()
            {
                Columns = new List<string>() { "Col" },
                Rows = new List<List<object>>()
                {
                    new() { "Row" }
                }
            });

        
        // act
        var result = sut.GetDatabaseInfo(default!);

        // assert
        result.Result!.Schemas.Count.Should().Be(1);
    }

    [Theory, AutoMoqData]
    public void GetDatabaseInfo_ExecutorThrows_ReturnsResultNotOk([Frozen] Mock<ICommandExecutor> executor,
        DatabaseInfoService sut)
    {
        // arrange
        executor.Setup(ex => ex.Execute(It.IsAny<string>(), It.IsAny<IDbConnection>())).Throws(new Exception());

        
        // act
        var result = sut.GetDatabaseInfo(default!);


        // assert
        result.Ok.Should().BeFalse();
    }
}