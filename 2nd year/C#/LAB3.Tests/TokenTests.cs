using TextAnalysis.Tokens;
using Xunit;

namespace Lab3.Tests;

public class TokenTests
{
    [Fact]
    public void Word_StoresValue_AndExposesLength()
    {
        var w = new Word("hello");

        Assert.Equal("hello", w.Value);
        Assert.Equal(5, w.Length);
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData("a", 1)]
    [InlineData("Привет", 6)]
    [InlineData("Hello", 5)]
    [InlineData("123", 3)]
    public void Word_Length_MatchesStringLength(string value, int expected)
    {
        Assert.Equal(expected, new Word(value).Length);
    }

    [Theory]
    [InlineData(".")]
    [InlineData(",")]
    [InlineData("!")]
    [InlineData("?")]
    [InlineData("—")]
    [InlineData("–")]
    public void Punctuation_StoresValue_AndExposesLength(string p)
    {
        var token = new Punctuation(p);

        Assert.Equal(p, token.Value);
        Assert.Equal(p.Length, token.Length);
    }

    [Fact]
    public void Word_IsAssignableToToken()
    {
        Token t = new Word("hi");
        Assert.IsType<Word>(t);
    }

    [Fact]
    public void Punctuation_IsAssignableToToken()
    {
        Token t = new Punctuation("!");
        Assert.IsType<Punctuation>(t);
    }
}