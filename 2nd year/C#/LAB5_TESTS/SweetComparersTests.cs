using ProductBuilder.Models;
using Xunit;

namespace ProductBuilder.Tests;

public class SweetComparersTests
{
    private static Lollipop MakeLollipop(string name, double sugar = 50)
        => new Lollipop(name, 10, sugar, 100, "вкус");

    [Fact]
    public void SugarComparer_SortsBySugar()
    {
        var sweets = new List<Sweet>
        {
            MakeLollipop("A", 80),
            MakeLollipop("B", 30),
            MakeLollipop("C", 55),
        };

        sweets.Sort(new SweetSugarComparer());

        Assert.Equal(new[] { "B", "C", "A" }, sweets.Select(s => s.Name).ToArray());
    }

    [Fact]
    public void NameComparer_SortsByName()
    {
        var sweets = new List<Sweet>
        {
            MakeLollipop("Charlie"),
            MakeLollipop("Alpha"),
            MakeLollipop("Bravo"),
        };

        sweets.Sort(new SweetNameComparer());

        Assert.Equal(new[] { "Alpha", "Bravo", "Charlie" },
            sweets.Select(s => s.Name).ToArray());
    }

    [Fact]
    public void SugarComparer_NullHandling()
    {
        var comparer = new SweetSugarComparer();
        var sweet = MakeLollipop("A");

        Assert.True(comparer.Compare(null, sweet) < 0);
        Assert.True(comparer.Compare(sweet, null) > 0);
        Assert.Equal(0, comparer.Compare(null, null));
    }

    [Fact]
    public void NameComparer_NullHandling()
    {
        var comparer = new SweetNameComparer();
        var sweet = MakeLollipop("A");

        Assert.True(comparer.Compare(null, sweet) < 0);
        Assert.True(comparer.Compare(sweet, null) > 0);
        Assert.Equal(0, comparer.Compare(null, null));
    }

    [Fact]
    public void SugarComparer_EqualValues_ReturnZero()
    {
        var comparer = new SweetSugarComparer();
        Assert.Equal(0, comparer.Compare(MakeLollipop("A", 50), MakeLollipop("B", 50)));
    }

    [Fact]
    public void NameComparer_EqualNames_ReturnZero()
    {
        var comparer = new SweetNameComparer();
        Assert.Equal(0, comparer.Compare(MakeLollipop("Same"), MakeLollipop("Same")));
    }
}