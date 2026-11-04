using System;
using System.IO;
using TextAnalysis.Reports;
using Xunit;

namespace Lab3.Tests;

public class ConcordanceTests
{
    private static string[] RunReporterAndReadLines(string sourceText)
    {
        string path = Path.GetTempFileName();
        try
        {
            var reporter = new ConcordanceReporter();
            reporter.GenerateAndSaveReport(sourceText, path);
            return File.ReadAllLines(path);
        }
        finally
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void GenerateReport_CreatesFile()
    {
        string path = Path.GetTempFileName();
        try
        {
            var reporter = new ConcordanceReporter();
            reporter.GenerateAndSaveReport("Hello world", path);

            Assert.True(File.Exists(path));
            Assert.True(new FileInfo(path).Length > 0);
        }
        finally
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void GenerateReport_BuildsCorrectConcordance()
    {
        string text =
            "Peter Piper picked a peck of pickled peppers\n" +
            "A peck of pickled peppers Peter Piper picked\n" +
            "If Peter Piper picked a peck of pickled peppers,\n" +
            "Where's the peck of pickled peppers Peter Piper picked?";

        var lines = RunReporterAndReadLines(text);

        
        Assert.Contains("a..................................3: 1 2 3", lines);
        Assert.Contains("if.................................1: 3", lines);
        Assert.Contains("of.................................4: 1 2 3 4", lines);
        Assert.Contains("peck...............................4: 1 2 3 4", lines);
        Assert.Contains("peppers............................4: 1 2 3 4", lines);
        Assert.Contains("peter..............................4: 1 2 3 4", lines);
        Assert.Contains("picked.............................4: 1 2 3 4", lines);
        Assert.Contains("pickled............................4: 1 2 3 4", lines);
        Assert.Contains("piper..............................4: 1 2 3 4", lines);
    }

    [Fact]
    public void GenerateReport_DoesNotDuplicateLineNumbers()
    {
        string text = "apple apple apple\nbanana";

        var lines = RunReporterAndReadLines(text);

        
        Assert.Contains("apple..............................3: 1", lines);
        Assert.Contains("banana.............................1: 2", lines);
    }

    [Fact]
    public void GenerateReport_IsCaseInsensitive()
    {
        string text = "Apple APPLE apple";

        var lines = RunReporterAndReadLines(text);

        
        Assert.Contains("apple..............................3: 1", lines);
    }

    [Fact]
    public void GenerateReport_IgnoresPunctuationAndSplitsWords()
    {
        string text = "\"Hello,\" said the cat.\nIt's a well-known fact!";

        var lines = RunReporterAndReadLines(text);

        
        Assert.Contains("hello..............................1: 1", lines);
        Assert.Contains("cat................................1: 1", lines);

        
        Assert.Contains("it.................................1: 2", lines);
        Assert.Contains("s..................................1: 2", lines);
        Assert.Contains("well...............................1: 2", lines);
        Assert.Contains("known..............................1: 2", lines);
    }

    [Fact]
    public void GenerateReport_HandlesRussianText()
    {
        string text = "Привет, мир!\nМир во всем мире.";

        var lines = RunReporterAndReadLines(text);

        Assert.Contains("во.................................1: 2", lines);
        Assert.Contains("всем...............................1: 2", lines);
        
        Assert.Contains("мир................................2: 1 2", lines);
        Assert.Contains("мире...............................1: 2", lines);
        Assert.Contains("привет.............................1: 1", lines);
    }

    [Fact]
    public void GenerateReport_EmptyText_ReturnsEmptyFile()
    {
        string text = "";

        var lines = RunReporterAndReadLines(text);

        Assert.Empty(lines);
    }

    [Fact]
    public void GenerateReport_HandlesMultipleEmptyLines()
    {
        string text = "apple\n\n\nbanana";

        var lines = RunReporterAndReadLines(text);

        
        Assert.Contains("apple..............................1: 1", lines);
        Assert.Contains("banana.............................1: 4", lines);
    }

    [Fact]
    public void GenerateReport_FormatsOutputWithDots()
    {
        var lines = RunReporterAndReadLines("test");

        
        Assert.Single(lines);
        Assert.Equal("test...............................1: 1", lines[0]);
        Assert.StartsWith("test.", lines[0]);
    }
}