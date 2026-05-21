using System.Text;
using FluentAssertions;
using ServiceLayer.DTW.Infrastructure.Parsing;

namespace ServiceLayer.DTW.Infrastructure.Tests;

public class CsvParserTests
{
    private readonly CsvParser _sut = new();

    [Fact]
    public async Task Should_Parse_Csv_Rows_Correctly()
    {
        const string csv = "CardCode,CardName,CardType\nC001,Acme Corp,C\nS001,Big Supplier,S";
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        var rows = await _sut.ParseAsync(stream, "test.csv");

        rows.Should().HaveCount(2);
        rows[0].Fields["CardCode"].Should().Be("C001");
        rows[1].Fields["CardType"].Should().Be("S");
    }

    [Theory]
    [InlineData("file.csv")]
    [InlineData("file.txt")]
    public void Should_Support_Csv_And_Txt(string fileName)
        => _sut.Supports(fileName).Should().BeTrue();

    [Fact]
    public void Should_Not_Support_Xlsx()
        => _sut.Supports("file.xlsx").Should().BeFalse();
}