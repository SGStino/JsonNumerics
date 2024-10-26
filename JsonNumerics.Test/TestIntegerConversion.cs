using JsonNumerics;
using System.Numerics;

namespace JsonNumerics.Test;

public class TestIntegerConversion
{
    [Theory]
    [InlineData("3.1415E4", 31415L)]
    [InlineData("3185786", 3185786L)]
    [InlineData("8603430864867486135", 8603430864867486135L)]
    [InlineData("-8603430864867486135", -8603430864867486135L)]
    public void TestToBigInteger(string input, BigInteger expected)
    {
        var number = JsonNumber.Parse(input);

        BigInteger bigInt = number;

        Assert.Equal(expected, bigInt);
    }
    [Theory]
    [InlineData("3.1415E4", 31415L)]
    [InlineData("3185786", 3185786L)]
    [InlineData("8603430864867486135", 8603430864867486135L)]
    [InlineData("-8603430864867486135", -8603430864867486135L)]
    public void TestToLong(string input, long expected)
    {
        var number = JsonNumber.Parse(input);

        long bigInt = number;

        Assert.Equal(expected, bigInt);
    }



    [Theory]
    [InlineData(31415L, "3.1415E4")]
    [InlineData(3185786L, "3185786")]
    [InlineData(8603430864867486135L, "8603430864867486135")]
    [InlineData(-8603430864867486135L, "-8603430864867486135")]
    public void TestFromInt64(long input, string expected)
    {
        JsonNumber number = input;

        Assert.Equal(number, JsonNumber.Parse(expected));
    }


    [Theory]
    [InlineData(31415, "3.1415E4")]
    [InlineData(3185786, "3185786")]
    [InlineData(2147483647, "2147483647")]
    [InlineData(-2147483647, "-2147483647")]
    public void TestFromInt32(int input, string expected)
    {
        JsonNumber number = input;
        Assert.Equal(number, JsonNumber.Parse(expected));
    }
    [Theory]
    [InlineData(31415, "3.1415E4")]
    [InlineData(3186, "3186")]
    [InlineData(32767, "32767")]
    [InlineData(-32767, "-32767")]
    public void TestFromInt16(short input, string expected)
    {
        JsonNumber number = input;
        Assert.Equal(number, JsonNumber.Parse(expected));
    }
}
