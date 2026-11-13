using ProductBuilder.Interfaces;
using ProductBuilder.Models;
using ProductBuilder.Services;
using Xunit;

namespace ProductBuilder.Tests;

public class GiftBuilderTests
{
    private static Lollipop MakeLollipop(string name = "Test")
        => new Lollipop(name, 10, 50, 100, "вкус");

    [Fact]
    public void Build_WithoutSetName_UsesDefaultName()
    {
        var gift = new GiftBuilder().Build();
        Assert.Equal("Новогодний подарок", gift.Name);
    }

    [Fact]
    public void SetName_SetsCustomName()
    {
        var gift = new GiftBuilder().SetName("Custom").Build();
        Assert.Equal("Custom", gift.Name);
    }

    [Fact]
    public void SetName_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => new GiftBuilder().SetName(null!));
    }

    [Fact]
    public void SetName_Empty_ThrowsOnBuild()
    {
        Assert.Throws<ArgumentException>(
            () => new GiftBuilder().SetName("").Build());
    }

    [Fact]
    public void AddSweet_AddsToGift()
    {
        var gift = new GiftBuilder()
            .AddSweet(MakeLollipop("A"))
            .AddSweet(MakeLollipop("B"))
            .Build();

        Assert.Equal(2, gift.Count);
    }

    [Fact]
    public void AddSweet_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => new GiftBuilder().AddSweet(null!));
    }

    [Fact]
    public void AddSweets_AddsRange()
    {
        var sweets = new[] { MakeLollipop("A"), MakeLollipop("B"), MakeLollipop("C") };
        var gift = new GiftBuilder().AddSweets(sweets).Build();
        Assert.Equal(3, gift.Count);
    }

    [Fact]
    public void AddSweets_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => new GiftBuilder().AddSweets(null!));
    }

    [Fact]
    public void Build_ResetsInternalState()
    {
        var builder = new GiftBuilder();
        builder.SetName("First").AddSweet(MakeLollipop()).Build();

        var second = builder.Build();

        Assert.Equal("Новогодний подарок", second.Name);
        Assert.Equal(0, second.Count);
    }

    [Fact]
    public void Builder_IsChainable()
    {
        var result = new GiftBuilder()
            .SetName("X")
            .AddSweet(MakeLollipop())
            .AddSweets(new[] { MakeLollipop() });

        Assert.IsAssignableFrom<IGiftBuilder>(result);
    }
}