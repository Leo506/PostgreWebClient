using System.Data;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Database;
using PostgreWebClient.Models;
using PostgreWebClient.UnitTests.FixtureAttributes;

namespace PostgreWebClient.UnitTests;

public class CommandServiceTests
{
    [Theory, AutoMoqData]
    public void ExecuteCommand_AllGood_ReturnsResultOk(CommandService sut)
    {
        // act
        var result = sut.ExecuteCommand("",default!);

        // assert
        result.Should().NotBeNull();
    }

    
    [Theory, AutoMoqData]
    public void ExecuteCommand_AllGood_ReturnsTable([Frozen] Mock<ICommandExecutor> executor, CommandService sut)
    {
        // arrange
        executor.Setup(commandExecutor => commandExecutor.Execute(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Returns(new Table()
            {
                Columns = new List<string>() { "Col" },
                Rows = new List<List<object>>()
                {
                    new() { 1 }
                }
            });
        
        // act
        var result = sut.ExecuteCommand("", default!);

        // assert
        result.Columns.Should().Contain("Col");
        result.Rows![0].Should().Contain(1);
    }

    [Theory, AutoMoqData]
    public void ExecuteCommand_ExecutorThrows_ReturnsSpecificTable([Frozen] Mock<ICommandExecutor> executor,
        CommandService sut)
    {
        // arrange
        executor.Setup(commandExecutor => commandExecutor.Execute(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Throws(new Exception("Error while execute query"));

        
        // act
        var result = sut.ExecuteCommand("query", default!);

        // assert
        result.Should().Be(new Table()
        {
            Columns = new List<string>() { "Query", "Result", "Reason" },
            Rows = new List<List<object>>()
            {
                new() { "query", "Failed", "Error while execute query" }
            }
        });
    }

    [Theory, AutoMoqData]
    public void ExecuteCommand_ExecutorReturnsEmptyTable_ReturnsSpecificTable([Frozen] Mock<ICommandExecutor> executor,
        CommandService sut)
    {
        // arrange
        executor.Setup(commandExecutor => commandExecutor.Execute(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Returns(new Table()
            {
                Columns = new List<string>(),
                Rows = new List<List<object>>()
            });
        
        // act
        var result = sut.ExecuteCommand("query", default!);
        
        // assert
        result.Should().Be(new Table()
        {
            Columns = new List<string>() { "Query", "Result" },
            Rows = new List<List<object>>()
            {
                new() { "query", "Success" }
            }
        });
    }
}