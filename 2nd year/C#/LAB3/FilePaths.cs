using System;

namespace TextAnalysis.Config
{
    public static class FilePaths
    {
        public static string Input { get; set; } = "input.txt";

        public static string Output { get; set; } = "output.xml";

        public static string StopWordsRu { get; set; } = "stopwords_ru.txt";

        public static string StopWordsEn { get; set; } = "stopwords_en.txt";

        public static bool Validate(bool verbose = true)
        {
            bool ok = true;
            ok &= Check(Input, "входной текст", verbose);
            ok &= Check(StopWordsRu, "стоп-слова (ru)", verbose);
            ok &= Check(StopWordsEn, "стоп-слова (en)", verbose);
            return ok;
        }

        private static bool Check(string path, string label, bool verbose)
        {
            bool exists = System.IO.File.Exists(path);
            if (!exists && verbose)
                Console.WriteLine($"(!) Не найден файл {label}: {path}");
            return exists;
        }
    }
}