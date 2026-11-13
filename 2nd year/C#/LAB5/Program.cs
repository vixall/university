using System.Text;
using ProductBuilder.Interfaces;
using ProductBuilder.Models;
using ProductBuilder.Services;

Console.OutputEncoding = Encoding.UTF8;

var dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "sweets.txt");
if (!File.Exists(dataPath))
    dataPath = Path.Combine("Data", "sweets.txt");

var catalog = SweetFactory.LoadFromFile(dataPath);

Console.WriteLine("=== Каталог сладостей ===");
foreach (var s in catalog)
    Console.WriteLine("  " + s);


IGiftBuilder builder = new GiftBuilder();
var gift = builder
    .SetName("Новогодний подарок для Васи")
    .AddSweets(catalog.Where(s => s.SugarPer100Grams < 80))
    .Build();

Console.WriteLine();
Console.WriteLine($"=== {gift.Name} ===");
Console.WriteLine($"Конфет: {gift.Count}");
Console.WriteLine($"Общий вес: {gift.TotalWeightGrams:F1} г");
Console.WriteLine($"Всего сахара: {gift.TotalSugarGrams:F1} г");
Console.WriteLine($"Стоимость: {gift.TotalPrice:F2} ₽");

Console.WriteLine();
Console.WriteLine("--- Содержимое ---");
foreach (var s in gift)
    Console.WriteLine("  " + s);


gift.SortByWeight();
Console.WriteLine();
Console.WriteLine("--- Сортировка по весу (IComparable) ---");
foreach (var s in gift)
    Console.WriteLine("  " + s);


gift.SortBySugar();
Console.WriteLine();
Console.WriteLine("--- Сортировка по сахару (IComparer) ---");
foreach (var s in gift)
    Console.WriteLine("  " + s);


const double minSugar = 60, maxSugar = 75;
Console.WriteLine();
Console.WriteLine($"--- Конфеты с сахаром от {minSugar} до {maxSugar} г/100г ---");
var found = gift.FindBySugarRange(minSugar, maxSugar).ToList();
if (found.Count == 0)
    Console.WriteLine("  Не найдено");
else
    foreach (var s in found)
        Console.WriteLine("  " + s);