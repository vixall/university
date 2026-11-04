using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TextAnalysis.Model;

namespace TextAnalysis.Reports
{
    //LAB_4
    public class ConcordanceReporter
    {
        public void GenerateAndSaveReport(string sourceText, string outputFilePath)
        {
            var concordance = new SortedDictionary<string, ConcordanceEntry>(StringComparer.OrdinalIgnoreCase);

            string[] lines = sourceText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                int lineNumber = i + 1;
                string line = lines[i];

                MatchCollection matches = Regex.Matches(line, @"\b[A-Za-zА-Яа-яЁё]+\b");

                foreach (Match match in matches)
                {
                    string word = match.Value.ToLowerInvariant();

                    if (!concordance.TryGetValue(word, out var entry))
                    {
                        entry = new ConcordanceEntry();
                        concordance[word] = entry;
                    }

                    entry.Count++;
                    entry.LineNumbers.Add(lineNumber);
                }
            }

            using (StreamWriter writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
            {
                foreach (var kvp in concordance)
                {
                    string word = kvp.Key;
                    int count = kvp.Value.Count;
                    string lineNumbers = string.Join(" ", kvp.Value.LineNumbers);

                    string formattedWord = word.PadRight(35, '.');

                    writer.WriteLine($"{formattedWord}{count}: {lineNumbers}");
                }
            }
        }
    }
    // end
}