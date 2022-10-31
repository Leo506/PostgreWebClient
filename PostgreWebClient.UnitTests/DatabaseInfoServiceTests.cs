﻿using System.Data;
using FluentAssertions;
using Moq;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Database;
using PostgreWebClient.Models;

namespace PostgreWebClient.UnitTests;

public class DatabaseInfoServiceTests
{
    [Fact]
    public void GetDatabaseInfo_AllGood_Returns_Info()
    {
        // arrange
        var executorMock = new Mock<ICommandExecutor>();
        executorMock.Setup(executor => executor.Execute()).Returns(new Table()
        {
            Columns = new List<string>() { "Column1", "Column2" },
            Rows = new List<List<object>>()
            {
                new() { "1", "2" },
                new() { "3", "4" }
            }
        });
        
        var factoryMock = new Mock<IExecutorFactory>();
        factoryMock.Setup(factory => factory.GetExecutor(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Returns(executorMock.Object);
        var sut = new DatabaseInfoService(factoryMock.Object);

        // act
        var result = sut.GetDatabaseInfo(default!);

        // assert
        result.Should().NotBeNull();
        result.Schemas.Count.Should().NotBe(0);
    }

    [Fact]
    public void GetDatabaseInfo_ExecutorThrows_Returns_Schemas_Empty()
    {
        // arrange
        var executorMock = new Mock<ICommandExecutor>();
        executorMock.Setup(executor => executor.Execute()).Throws(new Exception());

        var factoryMock = new Mock<IExecutorFactory>();
        factoryMock.Setup(factory => factory.GetExecutor(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Returns(executorMock.Object);

        var sut = new DatabaseInfoService(factoryMock.Object);

        // act
        var result = sut.GetDatabaseInfo(default!);


        // assert
        result.Schemas.Count.Should().Be(0);
    }

    [Fact]
    public void GetDatabaseInfo_ExecutorReturnsNoRows_Schemas_Empty()
    {
        // arrange
        var executorMock = new Mock<ICommandExecutor>();
        executorMock.Setup(executor => executor.Execute()).Returns(new Table()
        {
            Columns = new List<string>() { "Column" },
            Rows = new List<List<object>>()
        });

        var factoryMock = new Mock<IExecutorFactory>();
        factoryMock.Setup(factory => factory.GetExecutor(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Returns(executorMock.Object);

        var sut = new DatabaseInfoService(factoryMock.Object);

        // act
        var result = sut.GetDatabaseInfo(default!);


        // assert
        result.Schemas.Count.Should().Be(0);
    }

    [Fact]
    public void GetDatabaseInfo_ExecutorReturnsNoColumns_Schemas_Empty()
    {
        // arrange
        var executorMock = new Mock<ICommandExecutor>();
        executorMock.Setup(executor => executor.Execute()).Returns(new Table()
        {
            Columns = new List<string>(),
            Rows = new List<List<object>>()
        });

        var factoryMock = new Mock<IExecutorFactory>();
        factoryMock.Setup(factory => factory.GetExecutor(It.IsAny<string>(), It.IsAny<IDbConnection>()))
            .Returns(executorMock.Object);

        var sut = new DatabaseInfoService(factoryMock.Object);

        // act
        var result = sut.GetDatabaseInfo(default!);


        // assert
        result.Schemas.Count.Should().Be(0);
    }
}