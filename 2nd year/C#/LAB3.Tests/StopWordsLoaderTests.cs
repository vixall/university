using System;
using System.IO;
using TextAnalysis.StopWords;
using Xunit;

namespace Lab3.Tests;

public class StopWordsLoaderTests
{
    [Fact]
    public void Load_ReadsNonEmptyLines()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(path, new[] { "и", "в", "на", "  ", "" });

            var set = StopWordsLoader.Load(path);

            Assert.Contains("и", set);
            Assert.Contains("в", set);
            Assert.Contains("на", set);
            Assert.Equal(3, set.Count);
        }
        finally { File.Delete(path); }
    }

    [Fact]
    public void Load_MergesRuAndEn()
    {
        var ru = Path.GetTempFileName();
        var en = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(ru, new[] { "и", "в" });
            File.WriteAllLines(en, new[] { "the", "a" });

            var set = StopWordsLoader.Load(ru, en);

            Assert.Contains("и", set);
            Assert.Contains("в", set);
            Assert.Contains("the", set);
            Assert.Contains("a", set);
            Assert.Equal(4, set.Count);
        }
        finally { File.Delete(ru); File.Delete(en); }
    }

    [Fact]
    public void Load_IsCaseInsensitive()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(path, new[] { "The", "AND" });
            var set = StopWordsLoader.Load(path);

            Assert.Contains("the", set);
            Assert.Contains("THE", set);
            Assert.Contains("and", set);
            Assert.Contains("And", set);
        }
        finally { File.Delete(path); }
    }

    [Fact]
    public void Load_TrimsWhitespace()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(path, new[] { "  и  ", "\tв\t" });
            var set = StopWordsLoader.Load(path);

            Assert.Contains("и", set);
            Assert.Contains("в", set);
            Assert.Equal(2, set.Count);
        }
        finally { File.Delete(path); }
    }

    [Fact]
    public void Load_SkipsMissingFiles_ButLoadsExisting()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(path, new[] { "и", "в" });

            var set = StopWordsLoader.Load("no_such_file.txt", path);

            Assert.Contains("и", set);
            Assert.Contains("в", set);
        }
        finally { File.Delete(path); }
    }

    [Fact]
    public void Load_FallsBackToBuiltIn_WhenAllFilesMissing()
    {
        var set = StopWordsLoader.Load("no_such_file_1.txt", "no_such_file_2.txt");

        Assert.NotEmpty(set);
        Assert.Contains("is", set);
        Assert.Contains("and", set);
    }

    [Fact]
    public void Load_ReturnsUniqueSet_NoDuplicatesAcrossFiles()
    {
        var a = Path.GetTempFileName();
        var b = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(a, new[] { "и", "в" });
            File.WriteAllLines(b, new[] { "И", "в" });   // дубликаты по регистру

            var set = StopWordsLoader.Load(a, b);

            Assert.Equal(2, set.Count);
        }
        finally { File.Delete(a); File.Delete(b); }
    }
}