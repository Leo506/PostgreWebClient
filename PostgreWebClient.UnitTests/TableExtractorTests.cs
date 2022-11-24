using System.Data;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using PostgreWebClient.Extractors;
using PostgreWebClient.Models;
using PostgreWebClient.UnitTests.FixtureAttributes;

namespace PostgreWebClient.UnitTests;

public class TableExtractorTests
{
    [Theory, AutoMoqData]
    public void ExtractTable_AllGood_ReturnsTable([Frozen] Mock<IDataReader> reader, TableExtractor sut)
    {
        // arrange
        var expectedTable = new Table()
        {
            Columns = new List<string>() { "Col1" },
            Rows = new List<List<object>>()
            {
                new() { "Row" }
            }
        };

        reader.SetupGet(dataReader => dataReader.FieldCount).Returns(1);
        reader.Setup(dataReader => dataReader.GetName(0)).Returns("Col1");

        reader.Setup(dataReader => dataReader.Read()).Returns(true)
            .Callback(() => reader.Setup(r => r.Read()).Returns(false));
        reader.Setup(dataReader => dataReader[0]).Returns("Row");
        

        // act
        var result = sut.ExtractTable(reader.Object);

        // assert
        result.Equals(expectedTable).Should().BeTrue();
    }

    [Theory, AutoMoqData]
    public void ExtractTable_ReaderIsNull_RaiseException(TableExtractor sut)
    {
        // assert
        Assert.Throws<ArgumentNullException>(() => sut.ExtractTable(null));
    }
}