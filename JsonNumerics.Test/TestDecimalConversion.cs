using JsonNumerics;

namespace JsonNumerics.Test;

public class TestDecimalConversion
{
    private static void TestFromDecimal(decimal value, string expected)
    {
        var number = JsonNumber.FromDecimal(value);
        Assert.Equal(JsonNumber.Parse(expected), number);
    }

    private static void TestToDecimal(string number, decimal expected)
    {
        var dec = JsonNumber.Parse(number).ToDecimal();
        Assert.Equal(dec, expected);
    }

    [Fact] public void TestFromPi() => TestFromDecimal(3.1415926535897932384626433832m, "3.1415926535897932384626433832");
    [Fact] public void TestFromNegPi() => TestFromDecimal(-3.1415926535897932384626433832m, "-3.1415926535897932384626433832");
    [Fact] public void TestFromPosInt() => TestFromDecimal(123456789m, "123456789");
    [Fact] public void TestFromNegInt() => TestFromDecimal(-123456789m, "-123456789");
    [Fact] public void TestFromMaxValue() => TestFromDecimal(decimal.MaxValue, "79228162514264337593543950335");
    [Fact] public void TestFromMinValue() => TestFromDecimal(decimal.MinValue, "-79228162514264337593543950335");
    [Fact] public void TestFromEpsilon() => TestFromDecimal(1E-28m, "1E-28");



    [Fact] public void TestToPi() => TestToDecimal("3.1415926535897932384626433832", 3.1415926535897932384626433832m);
    [Fact] public void TestToNegPi() => TestToDecimal("-3.1415926535897932384626433832", -3.1415926535897932384626433832m);
    [Fact] public void TestToPosInt() => TestToDecimal("123456789", 123456789m);
    [Fact] public void TestToNegInt() => TestToDecimal("-123456789", -123456789m);
    [Fact] public void TestToMaxValue() => TestToDecimal("79228162514264337593543950335", decimal.MaxValue);
    [Fact] public void TestToMinValue() => TestToDecimal("-79228162514264337593543950335", decimal.MinValue);
    [Fact] public void TestToEpsilon() => TestToDecimal("1E-28", 1E-28m);

}
