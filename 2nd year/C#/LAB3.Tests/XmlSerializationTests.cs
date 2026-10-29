using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TextAnalysis.Model;
using TextAnalysis.Parsing;
using Xunit;

namespace Lab3.Tests;

public class XmlSerializationTests
{
    [Fact]
    public void ExportToXml_CreatesFile()
    {
        var path = Path.GetTempFileName();
        try
        {
            var text = TextParser.Parse("Hello world. Привет мир.");
            text.ExportToXml(path);

            Assert.True(File.Exists(path));
            Assert.True(new FileInfo(path).Length > 0);
        }
        finally { File.Delete(path); }
    }

    [Fact]
    public void ExportToXml_ProducesExpectedStructure()
    {
        var path = Path.GetTempFileName();
        try
        {
            var text = TextParser.Parse("a had lamb. Aesop and.");
            text.ExportToXml(path);

            var xml = File.ReadAllText(path);

            Assert.Contains("<text>", xml);
            Assert.Contains("<sentence>", xml);
            Assert.Contains("<word>a</word>", xml);
            Assert.Contains("<word>had</word>", xml);
            Assert.Contains("<word>lamb</word>", xml);
            Assert.Contains("<word>Aesop</word>", xml);
        }
        finally { File.Delete(path); }
    }

    [Fact]
    public void ExportToXml_IncludesOnlyWords_NotPunctuation()
    {
        var path = Path.GetTempFileName();
        try
        {
            var text = TextParser.Parse("Привет, мир!");
            text.ExportToXml(path);

            var xml = File.ReadAllText(path);

            Assert.DoesNotContain("<punct>", xml);
            Assert.DoesNotContain("<word>,</word>", xml);
            Assert.DoesNotContain("<word>!</word>", xml);
        }
        finally { File.Delete(path); }
    }

    [Fact]
    public void ExportToXml_EmptyText_ProducesValidEmptyXml()
    {
        var path = Path.GetTempFileName();
        try
        {
            var text = new Text();
            text.ExportToXml(path);

            var xml = File.ReadAllText(path);
            Assert.Contains("<text", xml);
            Assert.DoesNotContain("<sentence>", xml);
        }
        finally { File.Delete(path); }
    }

    [Fact]
    public void Xml_RoundTrip_PreservesSentencesAndWords()
    {
        var path = Path.GetTempFileName();
        try
        {
            var original = TextParser.Parse("Hello world. Как дела?");
            original.ExportToXml(path);

            using var fs = File.OpenRead(path);
            var restored = (Text)new XmlSerializer(typeof(Text)).Deserialize(fs)!;

            Assert.Equal(original.Sentences.Count, restored.Sentences.Count);

            for (int i = 0; i < original.Sentences.Count; i++)
            {
                var orig = original.Sentences[i].Words.Select(w => w.Value).ToList();
                var rest = restored.Sentences[i].Words.Select(w => w.Value).ToList();
                Assert.Equal(orig, rest);
            }
        }
        finally { File.Delete(path); }
    }

    [Fact]
    public void Xml_RoundTrip_RussianText()
    {
        var path = Path.GetTempFileName();
        try
        {
            var original = TextParser.Parse("Привет мир. Как дела?");
            original.ExportToXml(path);

            using var fs = File.OpenRead(path);
            var restored = (Text)new XmlSerializer(typeof(Text)).Deserialize(fs)!;

            Assert.Equal("Привет", restored.Sentences[0].Words.First().Value);
            Assert.Equal("Как", restored.Sentences[1].Words.First().Value);
        }
        finally { File.Delete(path); }
    }
}