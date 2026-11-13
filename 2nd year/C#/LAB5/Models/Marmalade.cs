namespace ProductBuilder.Models;

public class Marmalade : Candy
{
    public Marmalade(string name, double weight, double sugar, decimal pricePerKg, string flavor)
        : base(name, weight, sugar, pricePerKg, flavor) { }

    public override string GetDescription() => $"мармелад (вкус: {Flavor})";
}