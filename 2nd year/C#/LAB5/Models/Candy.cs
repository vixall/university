namespace ProductBuilder.Models;

public abstract class Candy : Sweet
{
    public string Flavor { get; }

    protected Candy(string name, double weight, double sugar, decimal pricePerKg, string flavor)
        : base(name, weight, sugar, pricePerKg)
    {
        Flavor = flavor;
    }
}