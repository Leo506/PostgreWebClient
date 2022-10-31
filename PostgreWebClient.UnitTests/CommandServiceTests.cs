using System.Data;
using FluentAssertions;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Database;
using PostgreWebClient.Models;

namespace PostgreWebClient.UnitTests;

public class CommandServiceTests
{
    [Fact]
    public void ExecuteCommand_AllGood_Returns_QueryModel()
    {
        // arrange
        var executorMock = new Mock<ICommandExecutor>();
        executorMock.Setup(executor => executor.Execute()).Returns(new Table()
        {
            Columns = new List<string>() { "Column" },
            Rows = new List<List<object>>()
            {
                new() { 1 },
                new() { 2 }
            }
        });
        var factoryMock = new Mock<IExecutorFactory>();
        factoryMock.Setup(factory => factory.GetExecutor(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Returns(executorMock.Object);
        var sut = new CommandService(factoryMock.Object);

        // act
        var result = sut.ExecuteCommand(new QueryModel()
        {
            QueryText = "SELECT 1"
        }, default!);

        // assert
        result.Headers.Should().NotBeNull();
        result.Headers!.Count.Should().NotBe(0);
        result.Rows.Should().NotBeNull();
        result.Rows!.Count.Should().NotBe(0);
    }

    [Fact]
    public void ExecuteCommand_ExecutorThrows_HasError_Set_True()
    {
        // arrange
        var executorMock = new Mock<ICommandExecutor>();
        executorMock.Setup(executor => executor.Execute()).Throws(new Exception());

        var factoryMock = new Mock<IExecutorFactory>();
        factoryMock.Setup(factory => factory.GetExecutor(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Returns(executorMock.Object);

        var sut = new CommandService(factoryMock.Object);

        // act
        var result = sut.ExecuteCommand(new QueryModel()
        {
            QueryText = "SELECT 1"
        }, default!);


        // assert
        result.HasError.Should().BeTrue();
    }
}