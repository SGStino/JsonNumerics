namespace JsonNumerics.Test;

public class TestComparison
{
    [Theory]
    [InlineData("3.1415", "3.1516", -1)]
    [InlineData("3.1415", "31516E-4", -1)] // different scales that need to re-align
    [InlineData("3E-28", "3E28", -1)]
    [InlineData("3000","3E3", 0)]
    [InlineData("30000", "3E3", 1)]
    [InlineData("3E4", "3000", 1)]
    public void TestCompare(string a, string b, int compare)
    {
        var numberA = JsonNumber.Parse(a);
        var numberB = JsonNumber.Parse(b);

        Assert.Equal(compare, numberA.CompareTo(numberB));
         
        if (compare < 0)
        {
            Assert.True(numberA < numberB);
            Assert.True(numberA <= numberB);
            Assert.True(numberA != numberB);
            Assert.False(numberA == numberB);
            Assert.False(numberA >= numberB); 
            Assert.False(numberA > numberB);
        }
        if (compare > 0)
        {
            Assert.False(numberA < numberB);
            Assert.False(numberA <= numberB);
            Assert.True(numberA != numberB);
            Assert.False(numberA == numberB);
            Assert.True(numberA >= numberB);
            Assert.True(numberA > numberB);
        }
        if(compare == 0)
        {
            Assert.False(numberA < numberB);
            Assert.True(numberA <= numberB);
            Assert.False(numberA != numberB);
            Assert.True(numberA == numberB);
            Assert.True(numberA >= numberB);
            Assert.False(numberA > numberB);
        }
    }

}