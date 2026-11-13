using ProductBuilder.Interfaces;
using ProductBuilder.Models;

namespace ProductBuilder.Services;

public class GiftBuilder : IGiftBuilder
{
    private const string DefaultName = "Новогодний подарок";
    private string _name = DefaultName;
    private readonly List<Sweet> _sweets = new();

    public IGiftBuilder SetName(string name)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        return this;
    }

    public IGiftBuilder AddSweet(Sweet sweet)
    {
        ArgumentNullException.ThrowIfNull(sweet);
        _sweets.Add(sweet);
        return this;
    }

    public IGiftBuilder AddSweets(IEnumerable<Sweet> sweets)
    {
        ArgumentNullException.ThrowIfNull(sweets);
        _sweets.AddRange(sweets);
        return this;
    }

    public Gift Build()
    {
        var gift = new Gift(_name);
        foreach (var s in _sweets) gift.Add(s);
        _sweets.Clear();
        _name = DefaultName;
        return gift;
    }
}