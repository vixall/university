using System.Linq;
using TextAnalysis.Model;
using TextAnalysis.Parsing;
using TextAnalysis.Tokens;
using Xunit;

namespace Lab3.Tests;

public class TextParserTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\n\n")]
    public void Parse_EmptyOrWhitespace_ReturnsEmptyText(string input)
    {
        var text = TextParser.Parse(input);
        Assert.Empty(text.Sentences);
    }

    [Fact]
    public void Parse_SingleSentence()
    {
        var text = TextParser.Parse("Hello, world!");

        Assert.Single(text.Sentences);
        Assert.Equal(2, text.Sentences[0].WordCount);
    }

    [Theory]
    [InlineData("One. Two. Three.", 3)]
    [InlineData("Один! Два? Три.", 3)]
    [InlineData("A. B. C. D.", 4)]
    [InlineData("Как дела? Хорошо.", 2)]
    public void Parse_MultipleSentences(string input, int expected)
    {
        var text = TextParser.Parse(input);
        Assert.Equal(expected, text.Sentences.Count);
    }

    [Fact]
    public void Parse_SplitsWordsAndPunctuation()
    {
        var text = TextParser.Parse("Привет, мир!");
        var tokens = text.Sentences[0].Tokens;

        Assert.Equal(4, tokens.Count);
        Assert.IsType<Word>(tokens[0]);
        Assert.Equal("Привет", tokens[0].Value);

        Assert.IsType<Punctuation>(tokens[1]);
        Assert.Equal(",", tokens[1].Value);

        Assert.IsType<Word>(tokens[2]);
        Assert.Equal("мир", tokens[2].Value);

        Assert.IsType<Punctuation>(tokens[3]);
        Assert.Equal("!", tokens[3].Value);
    }

    [Fact]
    public void Parse_DetectsQuestion()
    {
        var text = TextParser.Parse("Как дела? Хорошо.");

        Assert.True(text.Sentences[0].IsQuestion);
        Assert.False(text.Sentences[1].IsQuestion);
    }

    [Fact]
    public void Parse_RussianAndEnglishTogether()
    {
        var text = TextParser.Parse("Hello. Привет.");

        Assert.Equal(2, text.Sentences.Count);
        Assert.Equal("Hello", text.Sentences[0].Words.First().Value);
        Assert.Equal("Привет", text.Sentences[1].Words.First().Value);
    }

    [Fact]
    public void Parse_KeepsSentenceOrder()
    {
        var text = TextParser.Parse("Первый. Второй. Третий.");

        Assert.Equal("Первый", text.Sentences[0].Words.First().Value);
        Assert.Equal("Второй", text.Sentences[1].Words.First().Value);
        Assert.Equal("Третий", text.Sentences[2].Words.First().Value);
    }

    [Fact]
    public void Parse_HandlesDigits()
    {
        var text = TextParser.Parse("Test 123 end.");

        var values = text.Sentences[0].Words.Select(w => w.Value).ToList();
        Assert.Contains("123", values);
    }
}