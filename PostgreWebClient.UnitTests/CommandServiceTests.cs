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
    public void ExecuteCommand_AllGood_ReturnsTable([Frozen] Mock<IDbConnection> connection,
        [Frozen] Mock<ITableExtractor> extractor, CommandService sut)
    {
        // arrange
        var expectedTable = new Table()
        {
            Columns = new List<string>() { "Col1" },
            Rows = new List<List<object>>()
            {
                new() { "row" }
            }
        };
        extractor.Setup(tableExtractor => tableExtractor.ExtractTable(It.IsAny<IDataReader>())).Returns(expectedTable);

        // act
        var result = sut.ExecuteCommand("", connection.Object);

        // assert
        result.Equals(expectedTable)!.Should().BeTrue();

    }

    [Theory, AutoMoqData]
    public void ExecuteCommand_ExtractorThrows_ReturnsErrorTable([Frozen] Mock<IDbConnection> connection,
        [Frozen] Mock<ITableExtractor> extractor,
        CommandService sut)
    {
        // arrange
        extractor.Setup(ext => ext.ExtractTable(It.IsAny<IDataReader>())).Throws(new Exception("error"));


        // act
        var result = sut.ExecuteCommand("query", connection.Object);

        // assert
        result.Should().Be(Table.ErrorResult("query", "error"));
    }

    [Theory, AutoMoqData]
    public void ExecuteCommand_ExtractorReturnsEmptyTable_ReturnsSpecificTable([Frozen] Mock<IDbConnection> connection,
        [Frozen] Mock<ITableExtractor> extractor,
        CommandService sut)
    {
        // arrange
        extractor.Setup(ext => ext.ExtractTable(It.IsAny<IDataReader>())).Returns(Table.Empty);

        // act
        var result = sut.ExecuteCommand("query", connection.Object);

        // assert
        result.Should().Be(Table.SuccessResult("query"));
    }
}