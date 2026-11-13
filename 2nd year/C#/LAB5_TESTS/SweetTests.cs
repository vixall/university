using ProductBuilder.Models;
using Xunit;

namespace ProductBuilder.Tests;

public class SweetTests
{
    private sealed class TestSweet : Sweet
    {
        public TestSweet(string name, double weight, double sugar, decimal price)
            : base(name, weight, sugar, price) { }
        public override string GetDescription() => "test";
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidName_Throws(string? name)
    {
        Assert.Throws<ArgumentException>(() => new TestSweet(name!, 10, 50, 100));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_NonPositiveWeight_Throws(double weight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new TestSweet("Test", weight, 50, 100));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.1)]
    [InlineData(101)]
    [InlineData(150)]
    public void Constructor_InvalidSugar_Throws(double sugar)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new TestSweet("Test", 10, sugar, 100));
    }

    [Fact]
    public void Constructor_NegativePrice_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new TestSweet("Test", 10, 50, -1m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(50)]
    public void Constructor_ValidSugarBoundary_Allowed(double sugar)
    {
        var s = new TestSweet("Test", 10, sugar, 100);
        Assert.Equal(sugar, s.SugarPer100Grams);
    }

    [Fact]
    public void Properties_AreSetFromConstructor()
    {
        var s = new TestSweet("Конфета", 15, 55, 800);
        Assert.Equal("Конфета", s.Name);
        Assert.Equal(15.0, s.WeightGrams);
        Assert.Equal(55.0, s.SugarPer100Grams);
        Assert.Equal(800m, s.PricePerKilogram);
    }

    [Fact]
    public void SugarGrams_IsCalculatedCorrectly()
    {
        var s = new TestSweet("Test", 200, 50, 100);
        Assert.Equal(100.0, s.SugarGrams);
    }

    [Fact]
    public void Price_IsCalculatedCorrectly()
    {
        var s = new TestSweet("Test", 500, 50, 1000);
        Assert.Equal(500m, s.Price);
    }

    [Fact]
    public void CompareTo_ComparesByWeight()
    {
        var light = new TestSweet("Light", 10, 50, 100);
        var heavy = new TestSweet("Heavy", 100, 50, 100);
        var equal = new TestSweet("Equal", 10, 50, 100);

        Assert.True(light.CompareTo(heavy) < 0);
        Assert.True(heavy.CompareTo(light) > 0);
        Assert.Equal(0, light.CompareTo(equal));
    }

    [Fact]
    public void CompareTo_Null_ReturnsPositive()
    {
        var s = new TestSweet("Test", 10, 50, 100);
        Assert.Equal(1, s.CompareTo(null));
    }

    [Fact]
    public void CompareTo_SameReference_ReturnsZero()
    {
        var s = new TestSweet("Test", 10, 50, 100);
        Assert.Equal(0, s.CompareTo(s));
    }

    [Fact]
    public void ToString_ContainsKeyInfo()
    {
        var s = new TestSweet("Конфета", 15, 55, 800);
        var str = s.ToString();
        Assert.Contains("Конфета", str);
        Assert.Contains("15", str);
        Assert.Contains("55", str);
        Assert.Contains("test", str);
    }
}