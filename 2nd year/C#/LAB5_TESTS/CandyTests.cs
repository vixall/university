using Xunit;                  
using ProductBuilder.Models;   
using ProductBuilder.Services; 
using ProductBuilder.Interfaces;

namespace ProductBuilder.Tests;

public class CandyTests
{
    [Fact]
    public void Lollipop_StoresFlavorAndDescription()
    {
        var l = new Lollipop("Петушок", 12, 90, 300, "вишня");
        Assert.Equal("вишня", l.Flavor);
        Assert.Equal("леденец (вкус: вишня)", l.GetDescription());
    }

    [Fact]
    public void Marmalade_StoresFlavorAndDescription()
    {
        var m = new Marmalade("Яблочный ломтик", 10, 70, 450, "яблоко");
        Assert.Equal("яблоко", m.Flavor);
        Assert.Equal("мармелад (вкус: яблоко)", m.GetDescription());
    }

    [Fact]
    public void ChocolateCandy_StoresFlavorAndType()
    {
        var c = new ChocolateCandy("Алёнка", 20, 50, 950, "сливки", "тёмный");
        Assert.Equal("сливки", c.Flavor);
        Assert.Equal("тёмный", c.ChocolateType);
        Assert.Equal("шоколадная (тёмный, вкус: сливки)", c.GetDescription());
    }

    [Fact]
    public void CaramelCandy_WithFilling_Description()
    {
        var c = new CaramelCandy("Барбарис", 8, 75, 280, "барбарис", true);
        Assert.True(c.HasFilling);
        var d = c.GetDescription();
        Assert.Contains("с начинкой", d);
        Assert.Contains("барбарис", d);
    }

    [Fact]
    public void CaramelCandy_WithoutFilling_Description()
    {
        var c = new CaramelCandy("Дюшес", 9, 72, 320, "груша", false);
        Assert.False(c.HasFilling);
        Assert.DoesNotContain("с начинкой", c.GetDescription());
        Assert.Contains("груша", c.GetDescription());
    }
}