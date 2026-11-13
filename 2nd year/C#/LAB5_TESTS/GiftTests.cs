using ProductBuilder.Models;
using Xunit;

namespace ProductBuilder.Tests;

public class GiftTests
{
    private static Lollipop MakeLollipop(
        string name = "Test",
        double weight = 10,
        double sugar = 50,
        decimal price = 100,
        string flavor = "вкус")
        => new Lollipop(name, weight, sugar, price, flavor);

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidName_Throws(string? name)
    {
        Assert.Throws<ArgumentException>(() => new Gift(name!));
    }

    [Fact]
    public void Name_IsStored()
    {
        var gift = new Gift("Мой подарок");
        Assert.Equal("Мой подарок", gift.Name);
    }

    [Fact]
    public void Add_Null_Throws()
    {
        var gift = new Gift("Test");
        Assert.Throws<ArgumentNullException>(() => gift.Add(null!));
    }

    [Fact]
    public void Add_IncreasesCount()
    {
        var gift = new Gift("Test");
        gift.Add(MakeLollipop());
        gift.Add(MakeLollipop());
        Assert.Equal(2, gift.Count);
    }

    [Fact]
    public void Aggregates_AreComputedCorrectly()
    {
        var gift = new Gift("Test");
        gift.Add(new Lollipop("A", 100, 50, 1000, "x")); // 100g, 50g sugar, 100₽
        gift.Add(new Lollipop("B", 200, 25, 500, "y"));  // 200g, 50g sugar, 100₽

        Assert.Equal(300.0, gift.TotalWeightGrams);
        Assert.Equal(100.0, gift.TotalSugarGrams);
        Assert.Equal(200m, gift.TotalPrice);
    }

    [Fact]
    public void SortByWeight_SortsAscending()
    {
        var gift = new Gift("Test");
        gift.Add(MakeLollipop("Heavy", 100));
        gift.Add(MakeLollipop("Light", 10));
        gift.Add(MakeLollipop("Medium", 50));

        gift.SortByWeight();

        Assert.Equal(
            new[] { "Light", "Medium", "Heavy" },
            gift.Sweets.Select(s => s.Name).ToArray());
    }

    [Fact]
    public void SortBySugar_SortsAscending()
    {
        var gift = new Gift("Test");
        gift.Add(MakeLollipop("High", sugar: 90));
        gift.Add(MakeLollipop("Low", sugar: 20));
        gift.Add(MakeLollipop("Mid", sugar: 50));

        gift.SortBySugar();

        Assert.Equal(
            new[] { "Low", "Mid", "High" },
            gift.Sweets.Select(s => s.Name).ToArray());
    }

    [Fact]
    public void SortByName_SortsAlphabetically()
    {
        var gift = new Gift("Test");
        gift.Add(MakeLollipop("C"));
        gift.Add(MakeLollipop("A"));
        gift.Add(MakeLollipop("B"));

        gift.SortByName();

        Assert.Equal(
            new[] { "A", "B", "C" },
            gift.Sweets.Select(s => s.Name).ToArray());
    }

    [Fact]
    public void FindBySugarRange_ReturnsMatches()
    {
        var gift = new Gift("Test");
        gift.Add(MakeLollipop("A", sugar: 50));
        gift.Add(MakeLollipop("B", sugar: 70));
        gift.Add(MakeLollipop("C", sugar: 90));

        var found = gift.FindBySugarRange(40, 80).ToList();

        Assert.Equal(2, found.Count);
        Assert.Equal(new[] { "A", "B" }, found.Select(s => s.Name).ToArray());
    }

    [Fact]
    public void FindBySugarRange_InclusiveBoundaries()
    {
        var gift = new Gift("Test");
        gift.Add(MakeLollipop("A", sugar: 50));
        gift.Add(MakeLollipop("B", sugar: 80));

        var found = gift.FindBySugarRange(50, 80).ToList();
        Assert.Equal(2, found.Count);
    }

    [Fact]
    public void FindBySugarRange_NoMatches_ReturnsEmpty()
    {
        var gift = new Gift("Test");
        gift.Add(MakeLollipop("A", sugar: 50));

        var found = gift.FindBySugarRange(60, 80).ToList();
        Assert.Empty(found);
    }

    [Fact]
    public void GetEnumerator_IteratesAllSweets()
    {
        var gift = new Gift("Test");
        gift.Add(MakeLollipop("A"));
        gift.Add(MakeLollipop("B"));

        var names = new List<string>();
        foreach (var s in gift) names.Add(s.Name);

        Assert.Equal(new[] { "A", "B" }, names);
    }
}