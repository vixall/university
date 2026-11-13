namespace ProductBuilder.Models;

public class ChocolateCandy : Candy
{
    public string ChocolateType { get; }

    public ChocolateCandy(string name, double weight, double sugar, decimal pricePerKg,
                          string flavor, string chocolateType)
        : base(name, weight, sugar, pricePerKg, flavor)
        => ChocolateType = chocolateType;

    public override string GetDescription() =>
        $"шоколадная ({ChocolateType}, вкус: {Flavor})";
}