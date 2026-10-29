using System;
using System.IO;
using System.Linq;
using System.Text;
using TextAnalysis.Config;
using TextAnalysis.Model;
using TextAnalysis.Parsing;
using TextAnalysis.StopWords;

namespace TextAnalysis
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            if (!File.Exists(FilePaths.Input))
            {
                Console.WriteLine($"Ошибка: входной файл не найден: {FilePaths.Input}");
                return;
            }
            FilePaths.Validate();   

            string source = File.ReadAllText(FilePaths.Input);
            var text = TextParser.Parse(source);

            Console.WriteLine($"Прочитан файл: {FilePaths.Input}");
            Console.WriteLine($"Предложений:   {text.Sentences.Count}");

            
            Console.WriteLine("\n===== 1. Предложения по возрастанию количества слов =====");
            foreach (var s in text.GetSentencesByWordCount())
                Console.WriteLine($"  [{s.WordCount,2}] {s}");

            
            Console.WriteLine("\n===== 2. Предложения по возрастанию длины =====");
            foreach (var s in text.GetSentencesByLength())
                Console.WriteLine($"  [{s.Length,3}] {s}");

            
            Console.WriteLine("\n===== 3. Слова длины 3 в вопросительных предложениях =====");
            var found = text.FindWordsInQuestions(3).ToList();
            if (found.Count == 0) Console.WriteLine("  (не найдено)");
            foreach (var w in found) Console.WriteLine($"  {w}");

            
            Console.WriteLine("\n===== 4. Удаление слов длины 3, начинающихся с согласной =====");
            text.RemoveConsonantWords(3);
            foreach (var s in text.Sentences) Console.WriteLine($"  {s}");

            
            Console.WriteLine("\n===== 5. Замена в предложении #1 слов длины 4 на 'X' =====");
            if (text.Sentences.Count > 1)
            {
                Console.WriteLine($"  ДО   : {text.Sentences[1]}");
                text.ReplaceWordsInSentence(1, 4, "X");
                Console.WriteLine($"  ПОСЛЕ: {text.Sentences[1]}");
            }

            
            Console.WriteLine("\n===== 6. Удаление стоп-слов =====");
            var stops = StopWordsLoader.Load();
            text.RemoveStopWords(stops);
            foreach (var s in text.Sentences) Console.WriteLine($"  {s}");

            
            Console.WriteLine("\n===== 7. Экспорт в XML =====");
            text.ExportToXml(FilePaths.Output);
            Console.WriteLine($"Результат записан в: {FilePaths.Output}");
        }
    }
}