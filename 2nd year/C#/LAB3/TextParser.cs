using System.Text.RegularExpressions;
using TextAnalysis.Model;
using TextAnalysis.Tokens;

namespace TextAnalysis.Parsing
{
    public static class TextParser
    {
        private static readonly Regex SentenceRegex =
            new Regex(@"[^.!?]+[.!?]*", RegexOptions.Compiled);

        private static readonly Regex TokenRegex =
            new Regex(@"[A-Za-zА-Яа-яЁё0-9]+|[^\sA-Za-zА-Яа-яЁё0-9]", RegexOptions.Compiled);

        private static readonly Regex IsWordRegex =
            new Regex(@"^[A-Za-zА-Яа-яЁё0-9]+$", RegexOptions.Compiled);

        public static Text Parse(string content)
        {
            var text = new Text();
            foreach (Match m in SentenceRegex.Matches(content))
            {
                var s = m.Value.Trim();
                if (string.IsNullOrWhiteSpace(s)) continue;

                var sentence = ParseSentence(s);
                if (sentence.Tokens.Count > 0)
                    text.Sentences.Add(sentence);
            }
            return text;
        }

        private static Sentence ParseSentence(string s)
        {
            var sentence = new Sentence();
            foreach (Match m in TokenRegex.Matches(s))
            {
                var v = m.Value;
                if (IsWordRegex.IsMatch(v))
                    sentence.Tokens.Add(new Word(v));
                else
                    sentence.Tokens.Add(new Punctuation(v));
            }
            return sentence;
        }
    }
}