using System;
using System.Collections.Generic;
using System.IO;
using TextAnalysis.Config;

namespace TextAnalysis.StopWords
{
    public static class StopWordsLoader
    {
        private static readonly HashSet<string> Fallback = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "это", "как", "очень", "is", "a", "an", "and", "this", "very", "are", "you", "how"
        };

        public static HashSet<string> Load() =>
            Load(FilePaths.StopWordsRu, FilePaths.StopWordsEn);

        public static HashSet<string> Load(params string[] paths)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            bool loaded = false;

            foreach (var p in paths)
            {
                if (string.IsNullOrWhiteSpace(p) || !File.Exists(p)) continue;

                foreach (var line in File.ReadAllLines(p))
                {
                    var w = line.Trim().ToLowerInvariant();
                    if (w.Length > 0) set.Add(w);
                }
                loaded = true;
            }

            if (!loaded)
            {
                Console.WriteLine("(!) Файлы стоп-слов не найдены — используется встроенный набор.");
                foreach (var w in Fallback) set.Add(w);
            }
            return set;
        }
    }
}