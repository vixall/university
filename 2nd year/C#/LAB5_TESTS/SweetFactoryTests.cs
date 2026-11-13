using ProductBuilder.Models;
using ProductBuilder.Services;
using Xunit;

namespace ProductBuilder.Tests;

public class SweetFactoryTests
{
    [Fact]
    public void CreateFromLine_Chocolate_CreatesChocolateCandy()
    {
        var s = SweetFactory.CreateFromLine(
            "Chocolate|Мишка косолапый|15|55|800|ваниль|молочный");

        var c = Assert.IsType<ChocolateCandy>(s);
        Assert.Equal("Мишка косолапый", c.Name);
        Assert.Equal(15.0, c.WeightGrams);
        Assert.Equal(55.0, c.SugarPer100Grams);
        Assert.Equal(800m, c.PricePerKilogram);
        Assert.Equal("ваниль", c.Flavor);
        Assert.Equal("молочный", c.ChocolateType);
    }

    [Fact]
    public void CreateFromLine_Chocolate_MissingType_UsesDefault()
    {
        var c = Assert.IsType<ChocolateCandy>(
            SweetFactory.CreateFromLine("Chocolate|A|10|50|100|ваниль"));
        Assert.Equal("молочный", c.ChocolateType);
    }

    [Fact]
    public void CreateFromLine_Chocolate_MissingFlavor_UsesDefault()
    {
        var c = Assert.IsType<ChocolateCandy>(
            SweetFactory.CreateFromLine("Chocolate|A|10|50|100"));
        Assert.Equal("ваниль", c.Flavor);
    }

    [Fact]
    public void CreateFromLine_Lollipop_CreatesLollipop()
    {
        var s = SweetFactory.CreateFromLine("Lollipop|Петушок|12|90|300|вишня");
        var l = Assert.IsType<Lollipop>(s);
        Assert.Equal("Петушок", l.Name);
        Assert.Equal("вишня", l.Flavor);
    }

    [Fact]
    public void CreateFromLine_Lollipop_MissingFlavor_UsesDefault()
    {
        var l = Assert.IsType<Lollipop>(
            SweetFactory.CreateFromLine("Lollipop|A|10|50|100"));
        Assert.Equal("клубника", l.Flavor);
    }

    [Fact]
    public void CreateFromLine_Marmalade_CreatesMarmalade()
    {
        var s = SweetFactory.CreateFromLine("Marmalade|Яблочный|10|70|450|яблоко");
        var m = Assert.IsType<Marmalade>(s);
        Assert.Equal("яблоко", m.Flavor);
    }

    [Fact]
    public void CreateFromLine_Marmalade_MissingFlavor_UsesDefault()
    {
        var m = Assert.IsType<Marmalade>(
            SweetFactory.CreateFromLine("Marmalade|A|10|50|100"));
        Assert.Equal("яблоко", m.Flavor);
    }

    [Theory]
    [InlineData("Caramel|Барбарис|8|75|280|барбарис|true", true)]
    [InlineData("Caramel|Барбарис|8|75|280|барбарис|false", false)]
    [InlineData("Caramel|Дюшес|9|72|320|груша", false)]
    public void CreateFromLine_Caramel_ParsesHasFilling(string line, bool expected)
    {
        var c = Assert.IsType<CaramelCandy>(SweetFactory.CreateFromLine(line));
        Assert.Equal(expected, c.HasFilling);
    }

    [Fact]
    public void CreateFromLine_Caramel_MissingFlavor_UsesDefault()
    {
        var c = Assert.IsType<CaramelCandy>(
            SweetFactory.CreateFromLine("Caramel|A|10|50|100"));
        Assert.Equal("сливки", c.Flavor);
    }

    [Fact]
    public void CreateFromLine_UnknownType_Throws()
    {
        Assert.Throws<NotSupportedException>(
            () => SweetFactory.CreateFromLine("Unknown|Name|10|50|100|flavor"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Lollipop")]
    [InlineData("Lollipop|Name")]
    [InlineData("Lollipop|Name|10")]
    [InlineData("Lollipop|Name|10|50")]
    public void CreateFromLine_TooFewParts_Throws(string line)
    {
        Assert.Throws<FormatException>(() => SweetFactory.CreateFromLine(line));
    }

    [Fact]
    public void LoadFromFile_LoadsAllSweets()
    {
        var path = WriteTempFile(
            "# комментарий\n" +
            "Lollipop|Петушок|12|90|300|вишня\n" +
            "Marmalade|Яблочный|10|70|450|яблоко\n");

        try
        {
            var sweets = SweetFactory.LoadFromFile(path);
            Assert.Equal(2, sweets.Count);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void LoadFromFile_SkipsCommentsAndBlankLines()
    {
        var path = WriteTempFile(
            "# Header\n" +
            "\n" +
            "   \n" +
            "  # indented comment\n" +
            "Lollipop|Test|10|50|100|вкус\n" +
            "\n");

        try
        {
            var sweets = SweetFactory.LoadFromFile(path);
            Assert.Single(sweets);
            Assert.Equal("Test", sweets[0].Name);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void LoadFromFile_MissingFile_Throws()
    {
        var nonexistent = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid().ToString("N") + ".txt");

        Assert.Throws<FileNotFoundException>(
            () => SweetFactory.LoadFromFile(nonexistent));
    }

    private static string WriteTempFile(string content)
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, content);
        return path;
    }
}