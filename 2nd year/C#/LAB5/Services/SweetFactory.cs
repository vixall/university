using ProductBuilder.Models;

namespace ProductBuilder.Services;

public static class SweetFactory
{
    public static Sweet CreateFromLine(string line)
    {
        var p = line.Split('|', StringSplitOptions.TrimEntries);
        if (p.Length < 5)
            throw new FormatException($"Неверный формат строки: {line}");

        string type = p[0];
        string name = p[1];
        double weight = double.Parse(p[2]);
        double sugar = double.Parse(p[3]);
        decimal price = decimal.Parse(p[4]);

        return type switch
        {
            "Chocolate" => new ChocolateCandy(name, weight, sugar, price,
                                Get(p, 5, "ваниль"), Get(p, 6, "молочный")),
            "Lollipop" => new Lollipop(name, weight, sugar, price, Get(p, 5, "клубника")),
            "Marmalade" => new Marmalade(name, weight, sugar, price, Get(p, 5, "яблоко")),
            "Caramel" => new CaramelCandy(name, weight, sugar, price,
                                Get(p, 5, "сливки"), p.Length > 6 && bool.Parse(p[6])),
            _ => throw new NotSupportedException($"Неизвестный тип: {type}")
        };
    }

    private static string Get(string[] p, int i, string def) => p.Length > i ? p[i] : def;

    public static List<Sweet> LoadFromFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Файл не найден: {path}");

        return File.ReadAllLines(path)
            .Where(l => !string.IsNullOrWhiteSpace(l) && !l.TrimStart().StartsWith('#'))
            .Select(CreateFromLine)
            .ToList();
    }
}