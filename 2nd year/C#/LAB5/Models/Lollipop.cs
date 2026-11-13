namespace ProductBuilder.Models;

public class Lollipop : Candy
{
    public Lollipop(string name, double weight, double sugar, decimal pricePerKg, string flavor)
        : base(name, weight, sugar, pricePerKg, flavor) { }

    public override string GetDescription() => $"леденец (вкус: {Flavor})";
}