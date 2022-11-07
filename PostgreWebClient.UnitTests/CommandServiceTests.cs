using System.Data;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Database;
using PostgreWebClient.Factories;
using PostgreWebClient.Models;
using PostgreWebClient.UnitTests.FixtureAttributes;

namespace PostgreWebClient.UnitTests;

public class CommandServiceTests
{
    [Theory, AutoMoqData]
    public void ExecuteCommand_AllGood_ReturnsResultOk([Frozen] Mock<ICommandExecutor> executor, CommandService sut)
    {
        // arrange
        NpgsqlExecutorFactory.Executor = executor.Object;
        
        // act
        var result = sut.ExecuteCommand(new QueryModel(),default!);

        // assert
        result.Ok.Should().BeTrue();
    }

    
    // TODO: sometimes failed; need to find out why
    [Theory, AutoMoqData]
    public void ExecuteCommand_AllGood_ReturnsTable([Frozen] Mock<ICommandExecutor> executor, CommandService sut)
    {
        // arrange
        executor.Setup(commandExecutor => commandExecutor.Execute()).Returns(new Table()
        {
            Columns = new List<string>() { "Col" },
            Rows = new List<List<object>>()
            {
                new() { 1 }
            }
        });

        NpgsqlExecutorFactory.Executor = executor.Object;

        // act
        var result = sut.ExecuteCommand(new QueryModel(), default!);

        // assert
        result.Result!.Columns.Should().Contain("Col");
        result.Result!.Rows![0].Should().Contain(1);
    }

    [Theory, AutoMoqData]
    public void ExecuteCommand_ExecutorThrows_ReturnsResultNotOk([Frozen] Mock<ICommandExecutor> executor,
        CommandService sut)
    {
        // arrange
        executor.Setup(commandExecutor => commandExecutor.Execute()).Throws(new Exception());

        NpgsqlExecutorFactory.Executor = executor.Object;
        
        // act
        var result = sut.ExecuteCommand(new QueryModel(), default!);

        // assert
        result.Ok.Should().BeFalse();
    }
}