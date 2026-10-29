using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextAnalysis.Tokens;

namespace TextAnalysis.Model
{
    public class Sentence
    {
        public List<Token> Tokens { get; } = new List<Token>();

        public IEnumerable<Word> Words => Tokens.OfType<Word>();
        public int WordCount => Words.Count();
        public int Length => Tokens.Sum(t => t.Length);

        public bool IsQuestion => Tokens.OfType<Punctuation>().Any(p => p.Value == "?");

        private static bool SpaceBeforePunctuation(string p) => p == "—" || p == "–";

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var t in Tokens)
            {
                if (t is Word)
                {
                    if (sb.Length > 0) sb.Append(' ');
                    sb.Append(t.Value);
                }
                else
                {
                    if (SpaceBeforePunctuation(t.Value) && sb.Length > 0 && sb[^1] != ' ')
                        sb.Append(' ');
                    sb.Append(t.Value);
                }
            }
            return sb.ToString();
        }
    }
}