using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;

namespace JsonNumerics;
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public readonly struct JsonNumber(BigInteger integerPart, int scale) : IEquatable<JsonNumber>
{
    public readonly BigInteger IntegerPart => integerPart;
    /// <summary>
    /// Number of decimal places
    /// </summary>
    public readonly int Scale => scale;

    // Factory method to create from string
    public static JsonNumber Parse(string value)
    {
        if (value.Contains('e') || value.Contains('E'))
        {
            return ParseScientificNotation(value);
        }

        int scale = 0;
        if (value.Contains('.'))
        {
            scale = value.Length - value.IndexOf('.') - 1;
            value = value.Replace(".", "");
        }

        // Parse the resulting integer
        BigInteger integerPart = BigInteger.Parse(value.TrimStart('0')); // Remove leading zeroes
        return new JsonNumber(integerPart, scale);
    }


    // Parsing scientific notation like "1.23E-29"
    private static JsonNumber ParseScientificNotation(string value)
    {
        string[] parts = value.ToLower().Split('e');
        JsonNumber baseValue = Parse(parts[0]);
        int exponent = int.Parse(parts[1]);

        // Adjust the scale according to the exponent
        var scale = baseValue.Scale - exponent;
        return new(baseValue.IntegerPart, scale);
    }

    // Convert JsonNumerics to normalized string
    public override readonly string ToString()
    {
        var valueStr = new StringBuilder(integerPart.ToString());
        if (scale > 0)
        {
            // Adjust the position of the decimal point
            int decimalPosition = valueStr.Length - scale;
            if (decimalPosition > 0)
            {
                valueStr.Insert(decimalPosition, ".");
                for (var i = valueStr.Length - 1; i >= 0; i--)
                {
                    var c = valueStr[i];
                    if (c == '0')
                        valueStr.Remove(i, 1);
                    else break;
                }
                var end = valueStr.Length - 1;
                if (valueStr[end] == '.')
                    valueStr.Remove(end, 1);
            }
            else
            {
                // If the decimal point would go before the first digit
                valueStr.Insert(0, "0." + new string('0', -decimalPosition));
            }
        }
        else if (scale < 0)
        {
            if (scale > -3)
            {
                valueStr.Append('0', -scale);
            }
            else
            {
                valueStr.Append('E');
                valueStr.Append(-scale);
            }
        }

        return valueStr.ToString();
    }

    public bool IsNormalized => IntegerPart % 10 == 0;
    public readonly JsonNumber Normalized()
    {
        var mantissa = IntegerPart;
        var exponent = Scale;
        if (mantissa.IsZero)
        {
            exponent = 0;
        }
        else
        {
            BigInteger remainder = 0;
            while (remainder == 0)
            {
                var shortened = BigInteger.DivRem(mantissa, 10, out remainder);
                if (remainder == 0)
                {
                    mantissa = shortened;
                    exponent--;
                }
            }
        }

        return new JsonNumber(mantissa, exponent);
    }


    private string GetDebuggerDisplay() => $"{IntegerPart}E{-Scale}";

    #region Integer conversions
    // JsonNumber is BigInteger internally, everything can go through BigInteger conversions
    #region To Integer 
    public BigInteger ToInteger()
        => Scale switch
        {
            0 => IntegerPart,
            > 0 => IntegerPart * BigInteger.Pow(10, Scale),// Multiply by 10^Scale
            _ => IntegerPart / BigInteger.Pow(10, -Scale),// Divide by 10^(-Scale)
        };
    public static implicit operator BigInteger(JsonNumber number) => number.ToInteger();
    public static implicit operator UInt128(JsonNumber number) => (UInt128)number.ToInteger();
    public static implicit operator UInt64(JsonNumber number) => (UInt64)number.ToInteger();
    public static implicit operator UInt32(JsonNumber number) => (UInt32)number.ToInteger();
    public static implicit operator UInt16(JsonNumber number) => (UInt16)number.ToInteger();
    public static implicit operator byte(JsonNumber number) => (byte)number.ToInteger();
    public static implicit operator Int128(JsonNumber number) => (Int128)number.ToInteger();
    public static implicit operator Int64(JsonNumber number) => (Int64)number.ToInteger();
    public static implicit operator Int32(JsonNumber number) => (Int32)number.ToInteger();
    public static implicit operator Int16(JsonNumber number) => (Int16)number.ToInteger();
    public static implicit operator sbyte(JsonNumber number) => (sbyte)number.ToInteger();

    #endregion
    #region From Integer
    public static JsonNumber FromInteger(BigInteger other) => new(other, 0);
    public static implicit operator JsonNumber(BigInteger other) => FromInteger(other);
    public static implicit operator JsonNumber(UInt128 other) => FromInteger(other);
    public static implicit operator JsonNumber(UInt64 other) => FromInteger(other);
    public static implicit operator JsonNumber(UInt32 other) => FromInteger(other);
    public static implicit operator JsonNumber(UInt16 other) => FromInteger(other);
    public static implicit operator JsonNumber(byte other) => FromInteger(other);
    public static implicit operator JsonNumber(Int128 other) => FromInteger(other);
    public static implicit operator JsonNumber(Int64 other) => FromInteger(other);
    public static implicit operator JsonNumber(Int32 other) => FromInteger(other);
    public static implicit operator JsonNumber(Int16 other) => FromInteger(other);
    public static implicit operator JsonNumber(sbyte other) => FromInteger(other);
    #endregion
    #endregion


    #region Decimal Conversion


    public static JsonNumber FromDecimal(decimal value)
    {
        var scale = value.Scale;
        BigInteger integerPart;
        if (scale != 0)
            integerPart = (BigInteger)(value * (decimal)Math.Pow(10, scale));
        else
            integerPart = (BigInteger)(value);
        // Adjust scale to fit the JsonNumber representation (negative scale)
        return new JsonNumber(integerPart, scale);
    }


    public decimal ToDecimal() => Normalized().CreateDecimal();

    private decimal CreateDecimal()
    {
        if (Scale > 28) throw new OverflowException("Max scale for decimal is 28");
        // Check if IntegerPart is negative
        bool isNegative = IntegerPart < 0;

        // Use the absolute value for conversion
        BigInteger absIntegerPart = BigInteger.Abs(IntegerPart);

        // Break the BigInteger into its components

        var lo = (uint)(absIntegerPart & 0xFFFFFFFF); // Lower 32 bits
        var mid = (uint)((absIntegerPart >> 32) & 0xFFFFFFFF); // Next 32 bits
        var hi = (uint)((absIntegerPart >> 64) & 0xFFFFFFFF); // Upper 32 bits 

        // Create the decimal
        return new decimal((int)lo, (int)mid, (int)hi, isNegative, (byte)Scale);
    }
    #endregion

    #region Equality
    public static bool operator ==(JsonNumber left, JsonNumber right) => left.Equals(right);
    public static bool operator !=(JsonNumber left, JsonNumber right) => !left.Equals(right);

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is JsonNumber number
        && Equals(number);

    public bool Equals(JsonNumber other)
    {
        var normalized = Normalized();
        var otherNormalized = other.Normalized();
        return normalized.Scale == otherNormalized.Scale && normalized.IntegerPart == otherNormalized.IntegerPart;
    }

    public override int GetHashCode()
    {
        var normalized = Normalized();
        var hash = new HashCode();
        hash.Add(normalized.IntegerPart);
        hash.Add(normalized.Scale);
        return hash.ToHashCode();
    }
    #endregion
}