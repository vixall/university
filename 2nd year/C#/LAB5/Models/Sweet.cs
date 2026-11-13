using ProductBuilder.Interfaces;

namespace ProductBuilder.Models;

public abstract class Sweet : IPriceable, IComparable<Sweet>
{
    public string Name { get; }
    public double WeightGrams { get; }
    public double SugarPer100Grams { get; }
    public decimal PricePerKilogram { get; }

    protected Sweet(string name, double weightGrams, double sugarPer100Grams, decimal pricePerKilogram)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название не может быть пустым", nameof(name));
        if (weightGrams <= 0)
            throw new ArgumentOutOfRangeException(nameof(weightGrams), "Вес должен быть положительным");
        if (sugarPer100Grams is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(sugarPer100Grams));
        if (pricePerKilogram < 0)
            throw new ArgumentOutOfRangeException(nameof(pricePerKilogram));

        Name = name;
        WeightGrams = weightGrams;
        SugarPer100Grams = sugarPer100Grams;
        PricePerKilogram = pricePerKilogram;
    }

    public double SugarGrams => WeightGrams * SugarPer100Grams / 100.0;
    public decimal Price => PricePerKilogram * (decimal)WeightGrams / 1000m;

    public int CompareTo(Sweet? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return WeightGrams.CompareTo(other.WeightGrams);
    }

    public abstract string GetDescription();

    public override string ToString() =>
        $"{Name,-22} | {WeightGrams,5:F1} г | сахар {SugarPer100Grams,5:F1} г/100г | " +
        $"{Price,6:F2} ₽ | {GetDescription()}";
}