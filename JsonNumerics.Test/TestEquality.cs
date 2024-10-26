using JsonNumerics;

namespace JsonNumerics.Test;

public class TestEquality
{
    [Theory]
    [InlineData(15676, 4)]
    [InlineData(15676, -4)]
    [InlineData(4, 45678)]
    [InlineData(4, -8674)]
    public void TestIdenticalEquality(long integerPart, int scale)
    {
        var a = new JsonNumber(integerPart, scale);
        var b = new JsonNumber(integerPart, scale);

        Assert.True(a.Equals(b));
        Assert.True(a == b);
    }

    [Theory]
    [InlineData(15676, 4, 1567600,6)]
    [InlineData(15676, -4, 1567600,-2)]
    [InlineData(4, 45678, 40,45679)]
    [InlineData(4, -8674, 400, -8672)]
    public void TestDenormalizedEquality(long integerPartA, int scaleA, long integerPartB, int scaleB)
    {
        var a = new JsonNumber(integerPartA, scaleA);
        var b = new JsonNumber(integerPartB, scaleB);

        Assert.True(a.Equals(b));
        Assert.True(a == b);
    }
}
