using JsonNumerics;
using System.Numerics;
using Xunit.Abstractions;

namespace JsonNumerics.Test;

public class TestParsingAndFormatting(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [InlineData("1E10", 1, -10)]
    [InlineData("1E-10", 1, 10)]
    [InlineData("10E-10", 10, 10)]
    [InlineData("1.000E3", 1000, 0)]
    [InlineData("1.0000E3", 10000, 1)]
    public void TestScientificParse(string input, BigInteger integer, int scale)
    {

        var number = JsonNumber.Parse(input);
        Assert.Equal(integer, number.IntegerPart);
        Assert.Equal(scale, number.Scale);
    }


    [Theory]
    [InlineData("1.23456789")]
    [InlineData("12.3456789")]
    [InlineData("123.456789")]
    [InlineData("1234.56789")]
    [InlineData("12345.6789")]
    [InlineData("123456.789")]
    [InlineData("1234567.89")]
    [InlineData("12345678.9")]
    public void TestSymmetricRoundtrip(string input)
    {
        var number = JsonNumber.Parse(input);

        var result = number.ToString();

        testOutputHelper.WriteLine(result);
        Assert.Equal(input, result);
    }

    [Theory]
    [InlineData("1.23456789", "1.23456789")]
    [InlineData("1E3", "1E3")]
    [InlineData("1.000E3", "1000")]
    [InlineData("1.0000E3", "1000")]
    [InlineData("1234.56789", "1234.56789")]
    [InlineData("123456.789E-2", "1234.56789")]
    [InlineData("123456789E-8", "1.23456789")]
    [InlineData("123456789E-7", "12.3456789")]
    [InlineData("1.23456789E3", "1234.56789")]
    [InlineData("1.23456789000000", "1.23456789")]
    [InlineData("1.23456789000000000000E3", "1234.56789")]
    [InlineData("1E-3", "0.001")]
    [InlineData("1.234E-3", "0.001234")]
    [InlineData("1.234E5", "123400")]
    [InlineData("1234E2", "123400")]
    [InlineData("-1234E2", "-123400")]
    [InlineData("-1.234E5", "-123400")]
    public void TestRoundtrip(string input, string output)
    {
        var number = JsonNumber.Parse(input);

        var result = number.ToString();

        testOutputHelper.WriteLine(result);
        Assert.Equal(output, result);

    }

    [Theory]
    [InlineData("1.23456789", "1.23456789")]
    [InlineData("1E3", "1E3")]
    [InlineData("1.000E3", "1E3")]
    [InlineData("1.0000E3", "1E3")]
    [InlineData("1234.56789", "1234.56789")]
    [InlineData("123456.789E-2", "1234.56789")]
    [InlineData("123456789E-8", "1.23456789")]
    [InlineData("123456789E-7", "12.3456789")]
    [InlineData("1.23456789E3", "1234.56789")]
    [InlineData("1.23456789000000", "1.23456789")]
    [InlineData("1.23456789000000000000E3", "1234.56789")]
    [InlineData("1E-3", "0.001")]
    [InlineData("1.234E-3", "0.001234")]
    [InlineData("1.234E5", "123400")]
    [InlineData("1234E2", "123400")]
    [InlineData("-1234E2", "-123400")]
    [InlineData("-1.234E5", "-123400")]
    public void TestNormalizedRoundtrip(string input, string output)
    {
        var number = JsonNumber.Parse(input);

        var normalized = number.Normalized();

        var result = normalized.ToString();

        testOutputHelper.WriteLine(result);
        Assert.Equal(output, result);

    }

    [Theory]
    [InlineData("1.123456E29", "1123456E23")]
    public void TestLargeExponent(string input, string output)
    {
        var number = JsonNumber.Parse(input);

        var result = number.ToString();

        testOutputHelper.WriteLine(result);
        Assert.Equal(output, result);

    }
}