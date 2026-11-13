using ProductBuilder.Models;

namespace ProductBuilder.Interfaces;

public interface IGiftBuilder
{
    IGiftBuilder SetName(string name);
    IGiftBuilder AddSweet(Sweet sweet);
    IGiftBuilder AddSweets(IEnumerable<Sweet> sweets);
    Gift Build();
}