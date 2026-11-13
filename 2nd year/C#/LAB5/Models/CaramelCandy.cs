namespace ProductBuilder.Models;

public class CaramelCandy : Candy
{
    public bool HasFilling { get; }

    public CaramelCandy(string name, double weight, double sugar, decimal pricePerKg,
                        string flavor, bool hasFilling)
        : base(name, weight, sugar, pricePerKg, flavor)
        => HasFilling = hasFilling;

    public override string GetDescription() =>
        $"карамель (вкус: {Flavor}{(HasFilling ? ", с начинкой" : "")})";
}