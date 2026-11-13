using System.Collections;

namespace ProductBuilder.Models;

public class Gift : IEnumerable<Sweet>
{
    private readonly List<Sweet> _sweets = new();

    public string Name { get; }

    public Gift(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название подарка обязательно", nameof(name));
        Name = name;
    }

    public IReadOnlyList<Sweet> Sweets => _sweets;
    public int Count => _sweets.Count;
    public double TotalWeightGrams => _sweets.Sum(s => s.WeightGrams);
    public double TotalSugarGrams => _sweets.Sum(s => s.SugarGrams);
    public decimal TotalPrice => _sweets.Sum(s => s.Price);

    public void Add(Sweet sweet)
    {
        ArgumentNullException.ThrowIfNull(sweet);
        _sweets.Add(sweet);
    }

    public void SortByWeight() => _sweets.Sort();
    public void SortBySugar() => _sweets.Sort(new SweetSugarComparer());
    public void SortByName() => _sweets.Sort(new SweetNameComparer());

    public IEnumerable<Sweet> FindBySugarRange(double minPer100g, double maxPer100g) =>
        _sweets.Where(s => s.SugarPer100Grams >= minPer100g
                        && s.SugarPer100Grams <= maxPer100g);

    public IEnumerator<Sweet> GetEnumerator() => _sweets.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}