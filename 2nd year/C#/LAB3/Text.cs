using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using TextAnalysis.Tokens;

namespace TextAnalysis.Model
{
    [XmlRoot("text")]
    public class Text : IXmlSerializable
    {
        public List<Sentence> Sentences { get; private set; } = new List<Sentence>();

        
        public IEnumerable<Sentence> GetSentencesByWordCount()
            => Sentences.OrderBy(s => s.WordCount);

        public IEnumerable<Sentence> GetSentencesByLength()
            => Sentences.OrderBy(s => s.Length);
        public IEnumerable<string> FindWordsInQuestions(int length)
            => Sentences
                .Where(s => s.IsQuestion)
                .SelectMany(s => s.Words)
                .Where(w => w.Length == length)
                .Select(w => w.Value)
                .Distinct(StringComparer.OrdinalIgnoreCase);
        public void RemoveConsonantWords(int length)
        {
            foreach (var s in Sentences)
                s.Tokens.RemoveAll(t => t is Word w
                    && w.Length == length
                    && IsConsonant(w.Value[0]));
        }

        public void ReplaceWordsInSentence(int sentenceIndex, int wordLength, string replacement)
        {
            if (sentenceIndex < 0 || sentenceIndex >= Sentences.Count)
                throw new ArgumentOutOfRangeException(nameof(sentenceIndex));

            var s = Sentences[sentenceIndex];
            for (int i = 0; i < s.Tokens.Count; i++)
                if (s.Tokens[i] is Word w && w.Length == wordLength)
                    s.Tokens[i] = new Word(replacement);
        }

        public void RemoveStopWords(ISet<string> stopWords)
        {
            foreach (var s in Sentences)
                s.Tokens.RemoveAll(t => t is Word w
                    && stopWords.Contains(w.Value.ToLowerInvariant()));
        }

        public void ExportToXml(string path)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = new UTF8Encoding(false)
            };
            using var fs = new FileStream(path, FileMode.Create);
            using var writer = XmlWriter.Create(fs, settings);
            new XmlSerializer(typeof(Text)).Serialize(writer, this);
        }

        private const string RuConsonants = "бвгджзйклмнпрстфхцчшщ";
        private const string EnConsonants = "bcdfghjklmnpqrstvwxyz";

        public static bool IsConsonant(char c)
        {
            char l = char.ToLowerInvariant(c);
            return RuConsonants.IndexOf(l) >= 0 || EnConsonants.IndexOf(l) >= 0;
        }

        public XmlSchema GetSchema() => null;

        public void WriteXml(XmlWriter writer)
        {
            foreach (var sentence in Sentences)
            {
                writer.WriteStartElement("sentence");
                foreach (var word in sentence.Words)
                    writer.WriteElementString("word", word.Value);
                writer.WriteEndElement();
            }
        }

        public void ReadXml(XmlReader reader)
        {
            Sentences = new List<Sentence>();
            if (reader.IsEmptyElement) { reader.Read(); return; }
            reader.ReadStartElement();

            while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
            {
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "sentence")
                {
                    var sentence = new Sentence();
                    if (reader.IsEmptyElement) reader.Read();
                    else
                    {
                        reader.ReadStartElement();
                        while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "word")
                                sentence.Tokens.Add(new Word(reader.ReadElementContentAsString()));
                            else
                                reader.Read();
                        }
                        if (reader.NodeType == XmlNodeType.EndElement) reader.ReadEndElement();
                    }
                    Sentences.Add(sentence);
                }
                else reader.Read();
            }
            if (reader.NodeType == XmlNodeType.EndElement) reader.ReadEndElement();
        }
    }
}