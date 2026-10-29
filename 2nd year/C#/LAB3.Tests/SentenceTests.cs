using TextAnalysis.Model;
using TextAnalysis.Tokens;
using Xunit;

namespace Lab3.Tests;

public class SentenceTests
{
    private static Sentence Build(params Token[] tokens)
    {
        var s = new Sentence();
        s.Tokens.AddRange(tokens);
        return s;
    }

    [Fact]
    public void NewSentence_IsEmpty()
    {
        var s = new Sentence();

        Assert.Empty(s.Tokens);
        Assert.Equal(0, s.WordCount);
        Assert.Equal(0, s.Length);
        Assert.False(s.IsQuestion);
    }

    [Fact]
    public void WordCount_CountsOnlyWords()
    {
        var s = Build(
            new Word("Hello"), new Punctuation(","),
            new Word("world"), new Punctuation("!"));

        Assert.Equal(2, s.WordCount);
    }

    [Fact]
    public void Length_IsSumOfAllTokenLengths()
    {
        var s = Build(
            new Word("Hello"), new Punctuation(","),
            new Word("world"), new Punctuation("!"));

        Assert.Equal(5 + 1 + 5 + 1, s.Length);
    }

    [Fact]
    public void Words_ReturnsOnlyWordTokens()
    {
        var s = Build(
            new Word("a"), new Punctuation("."),
            new Word("b"));

        var words = s.Words.ToList();

        Assert.Equal(2, words.Count);
        Assert.Equal("a", words[0].Value);
        Assert.Equal("b", words[1].Value);
    }

    [Theory]
    [InlineData("?", true)]
    [InlineData("!", false)]
    [InlineData(".", false)]
    [InlineData(",", false)]
    public void IsQuestion_DetectsQuestionMark(string last, bool expected)
    {
        var s = Build(new Word("Hi"), new Punctuation(last));
        Assert.Equal(expected, s.IsQuestion);
    }

    [Fact]
    public void IsQuestion_ReturnsFalse_WithoutPunctuation()
    {
        var s = Build(new Word("Hi"));
        Assert.False(s.IsQuestion);
    }

    [Fact]
    public void ToString_JoinsWordsWithSpaces_AttachesCommaAndDot()
    {
        var s = Build(
            new Word("Hello"), new Punctuation(","),
            new Word("world"), new Punctuation("!"));

        Assert.Equal("Hello, world!", s.ToString());
    }

    [Fact]
    public void ToString_PutsSpacesAroundEmDash()
    {
        var s = Build(
            new Word("Программирование"),
            new Punctuation("—"),
            new Word("искусство"));

        Assert.Equal("Программирование — искусство", s.ToString());
    }

    [Fact]
    public void ToString_PutsSpacesAroundEnDash()
    {
        var s = Build(
            new Word("2010"),
            new Punctuation("–"),
            new Word("2020"));

        Assert.Equal("2010 – 2020", s.ToString());
    }

    [Fact]
    public void ToString_NoLeadingSpace_WhenSentenceStartsWithDash()
    {
        var s = Build(
            new Punctuation("—"),
            new Word("Привет"),
            new Punctuation("!"));

        Assert.Equal("— Привет!", s.ToString());
    }

    [Fact]
    public void ToString_NoDoubleSpace_BeforeDash()
    {
        var s = Build(
            new Word("Слово"),
            new Punctuation(" "),  
            new Punctuation("—"),
            new Word("дальше"));

        var result = s.ToString();
        Assert.DoesNotContain("  —", result);
    }

    
}