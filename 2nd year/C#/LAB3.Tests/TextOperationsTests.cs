using System.Collections.Generic;
using System.Linq;
using TextAnalysis.Model;
using TextAnalysis.Parsing;
using Xunit;

namespace Lab3.Tests;

public class TextOperationsTests
{
    // ========== п.1: по возрастанию количества слов ==========

    [Fact]
    public void GetSentencesByWordCount_SortsAscending()
    {
        var text = TextParser.Parse("One two three four. One. One two.");

        var counts = text.GetSentencesByWordCount().Select(s => s.WordCount).ToList();

        Assert.Equal(new[] { 1, 2, 4 }, counts);
    }

    [Fact]
    public void GetSentencesByWordCount_EmptyText_ReturnsEmpty()
    {
        var text = TextParser.Parse("");
        Assert.Empty(text.GetSentencesByWordCount());
    }

    [Fact]
    public void GetSentencesByWordCount_DoesNotMutateSentencesOrder()
    {
        var text = TextParser.Parse("One two three. One. One two.");
        var firstBefore = text.Sentences[0];

        _ = text.GetSentencesByWordCount().ToList();

        Assert.Same(firstBefore, text.Sentences[0]);
    }

    // ========== п.2: по возрастанию длины ==========

    [Fact]
    public void GetSentencesByLength_SortsAscending()
    {
        var text = TextParser.Parse("Hello world. A. Hi!");

        var lengths = text.GetSentencesByLength().Select(s => s.Length).ToList();

        Assert.Equal(lengths.OrderBy(x => x).ToList(), lengths);
    }

    [Fact]
    public void GetSentencesByLength_LongestIsLast()
    {
        var text = TextParser.Parse("A. Hello world! Hi.");
        var ordered = text.GetSentencesByLength().ToList();

        Assert.Equal(11, ordered[^1].Length); // "Hello world!" = 5+5+1
    }

    // ========== п.3: слова заданной длины в вопросах ==========

    [Fact]
    public void FindWordsInQuestions_ReturnsWordsFromQuestionsOnly()
    {
        var text = TextParser.Parse("Как дела? Всё как всегда.");

        var result = text.FindWordsInQuestions(3).ToList();

        Assert.Contains(result, w => string.Equals(w, "Как", System.StringComparison.OrdinalIgnoreCase));
        Assert.Single(result);
    }

    [Fact]
    public void FindWordsInQuestions_DoesNotDuplicate()
    {
        var text = TextParser.Parse("Как как как?");

        var result = text.FindWordsInQuestions(3).ToList();

        Assert.Single(result);
        Assert.Equal("Как", result[0], ignoreCase: true);
    }

    [Fact]
    public void FindWordsInQuestions_IsCaseInsensitive_Distinct()
    {
        var text = TextParser.Parse("Как КАК как?");

        var result = text.FindWordsInQuestions(3).ToList();

        Assert.Single(result);
    }

    [Fact]
    public void FindWordsInQuestions_ReturnsEmpty_WhenNoQuestionSentences()
    {
        var text = TextParser.Parse("Обычное предложение.");

        Assert.Empty(text.FindWordsInQuestions(3));
    }

    [Fact]
    public void FindWordsInQuestions_ReturnsEmpty_WhenLengthNotFound()
    {
        var text = TextParser.Parse("Как дела?");

        Assert.Empty(text.FindWordsInQuestions(10));
    }

    [Fact]
    public void FindWordsInQuestions_MultipleSentences_Flattened()
    {
        var text = TextParser.Parse("Как ты? Где был?");

        var result = text.FindWordsInQuestions(3)
            .Select(w => w.ToLowerInvariant())
            .OrderBy(w => w)
            .ToList();

        Assert.Equal(new[] { "был", "где", "как" }, result);
    }

    // ========== п.4: удалить слова длины N, начинающиеся с согласной ==========

    [Fact]
    public void RemoveConsonantWords_RemovesOnlyMatching()
    {
        var text = TextParser.Parse("Сок и оса тут. Мир!");
        text.RemoveConsonantWords(3);

        var remaining = text.Sentences
            .SelectMany(s => s.Words)
            .Select(w => w.Value)
            .ToList();

        // "Сок", "тут" и "Мир" должны быть удалены (длина 3, начинаются с согласной)
        Assert.DoesNotContain("Сок", remaining);
        Assert.DoesNotContain("тут", remaining);
        Assert.DoesNotContain("Мир", remaining); // ← исправлено на DoesNotContain

        // "оса" (гласная) и "и" (длина 1) должны остаться
        Assert.Contains("оса", remaining);
        Assert.Contains("и", remaining);
    }

    [Fact]
    public void RemoveConsonantWords_IgnoresOtherLengths()
    {
        var text = TextParser.Parse("стол столик столище");
        text.RemoveConsonantWords(4);

        var words = text.Sentences.SelectMany(s => s.Words).Select(w => w.Value).ToList();

        Assert.DoesNotContain("стол", words);
        Assert.Contains("столик", words);
        Assert.Contains("столище", words);
    }

    [Fact]
    public void RemoveConsonantWords_KeepsWordsStartingWithVowel()
    {
        var text = TextParser.Parse("оса окно ухо");
        text.RemoveConsonantWords(3);

        var words = text.Sentences.SelectMany(s => s.Words).Select(w => w.Value).ToList();

        Assert.Contains("оса", words);   // 'о' — гласная, длина 3 (осталось)
        Assert.Contains("ухо", words);   // 'у' — гласная, длина 3 (осталось)

        // "окно" имеет длину 4, поэтому оно не должно удаляться (и оно остается)
        Assert.Contains("окно", words);
    }

    [Fact]
    public void RemoveConsonantWords_EmptyText_DoesNothing()
    {
        var text = new Text();
        text.RemoveConsonantWords(3);

        Assert.Empty(text.Sentences);
    }

    // ========== п.5: замена в указанном предложении ==========

    [Fact]
    public void ReplaceWordsInSentence_ReplacesOnlyTargetSentence()
    {
        var text = TextParser.Parse("Один два три. Один два три.");

        text.ReplaceWordsInSentence(1, 3, "XXX");

        var first = text.Sentences[0].Words.Select(w => w.Value).ToList();
        var second = text.Sentences[1].Words.Select(w => w.Value).ToList();

        Assert.Contains("два", first);
        Assert.Contains("три", first);

        Assert.Equal(new[] { "Один", "XXX", "XXX" }, second);
    }

    [Fact]
    public void ReplaceWordsInSentence_AllowsReplacementOfDifferentLength()
    {
        var text = TextParser.Parse("Слово здесь.");
        text.ReplaceWordsInSentence(0, 5, "very very long replacement");

        Assert.Contains("very very long replacement",
            text.Sentences[0].Words.Select(w => w.Value));
    }

    [Fact]
    public void ReplaceWordsInSentence_DoesNotAffectPunctuation()
    {
        var text = TextParser.Parse("Слово здесь.");
        var punctBefore = text.Sentences[0].Tokens.OfType<TextAnalysis.Tokens.Punctuation>().Count();

        text.ReplaceWordsInSentence(0, 5, "x");

        var punctAfter = text.Sentences[0].Tokens.OfType<TextAnalysis.Tokens.Punctuation>().Count();
        Assert.Equal(punctBefore, punctAfter);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(5)]
    public void ReplaceWordsInSentence_ThrowsOnInvalidIndex(int index)
    {
        var text = TextParser.Parse("Одно предложение.");

        Assert.Throws<System.ArgumentOutOfRangeException>(
            () => text.ReplaceWordsInSentence(index, 3, "x"));
    }

    // ========== п.6: удаление стоп-слов ==========

    [Fact]
    public void RemoveStopWords_RemovesCaseInsensitive()
    {
        var text = TextParser.Parse("Это очень важный Это текст.");
        var stops = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
        {
            "это", "очень"
        };

        text.RemoveStopWords(stops);

        var remaining = text.Sentences
            .SelectMany(s => s.Words)
            .Select(w => w.Value)
            .ToList();

        Assert.DoesNotContain(remaining, w => string.Equals(w, "это", System.StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(remaining, w => string.Equals(w, "очень", System.StringComparison.OrdinalIgnoreCase));
        Assert.Contains("важный", remaining);
        Assert.Contains("текст", remaining);
    }

    [Fact]
    public void RemoveStopWords_EmptySet_DoesNothing()
    {
        var text = TextParser.Parse("Hello world.");
        var before = text.Sentences.Sum(s => s.WordCount);

        text.RemoveStopWords(new HashSet<string>());

        Assert.Equal(before, text.Sentences.Sum(s => s.WordCount));
    }

    [Fact]
    public void RemoveStopWords_RemovesMultipleWords()
    {
        var text = TextParser.Parse("и в на и в на конец");
        var stops = new HashSet<string> { "и", "в", "на" };

        text.RemoveStopWords(stops);

        var words = text.Sentences.SelectMany(s => s.Words).Select(w => w.Value).ToList();
        Assert.Equal(new[] { "конец" }, words);
    }

    [Fact]
    public void RemoveStopWords_DoesNotTouchPunctuation()
    {
        var text = TextParser.Parse("и, в.");
        var stops = new HashSet<string> { "и", "в" };

        text.RemoveStopWords(stops);

        var punct = text.Sentences[0].Tokens
            .OfType<TextAnalysis.Tokens.Punctuation>()
            .Select(p => p.Value)
            .ToList();

        Assert.Equal(new[] { ",", "." }, punct);
    }
}