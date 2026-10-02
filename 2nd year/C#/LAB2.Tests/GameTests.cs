using System;
using System.IO;
using Xunit;

public class GameTests : IDisposable
{
    private readonly string _inputPath;
    private readonly string _outputPath;

    public GameTests()
    {
        _inputPath = Path.Combine(Path.GetTempPath(), $"lab2_in_{Guid.NewGuid()}.txt");
        _outputPath = Path.Combine(Path.GetTempPath(), $"lab2_out_{Guid.NewGuid()}.txt");
        Game.InputFile = _inputPath;
        Game.OutFile = _outputPath;
    }

    public void Dispose()
    {
        if (File.Exists(_inputPath)) File.Delete(_inputPath);
        if (File.Exists(_outputPath)) File.Delete(_outputPath);
    }

    private string[] RunGame(int size, params string[] lines)
    {
        File.WriteAllLines(_inputPath, lines);
        var game = new Game(size);
        game.Run();
        return File.ReadAllLines(_outputPath);
    }

    [Fact]
    public void InitialState_PlayersNotInGame()
    {
        string[] output = RunGame(16, "16", "P");
        string? tableLine = Array.Find(output, l => l.Contains("??"));
        Assert.NotNull(tableLine);
    }

    [Fact]
    public void Cat_Catches_Mouse_Scenario_1()
    {
        string[] input = {
            "16",
            "M         7",
            "M        -5",
            "P",
            "C         6",
            "M        -7",
            "P",
            "C         6",
            "P",
            "M         4",
            "M         6",
            "C         0",
            "P",
            "M         0",
            "M         6",
            "P",
            "C        -1",
            "P",
            "C         1",
            "C         4",
            "P",
            "M        -4",
            "P"
        };

        string[] output = RunGame(16, input);

        Assert.Contains("Cat and Mouse", output);
        Assert.Contains("Mouse caught at: 11", output);

        int idx = Array.FindIndex(output, l => l.StartsWith("Distance traveled:"));
        Assert.True(idx >= 0);
        string numbersLine = output[idx + 1];
        Assert.Contains("28", numbersLine);
        Assert.Contains("7", numbersLine);
    }

    [Fact]
    public void Mouse_Evades_Scenario_2()
    {
        string[] input = {
            "20",
            "M        19",
            "M        -4",
            "P",
            "M        -1",
            "P",
            "M         3",
            "P",
            "M         0",
            "P",
            "C         3",
            "M        -6",
            "P",
            "M         2",
            "P",
            "C         1",
            "P",
            "C         6",
            "M         0",
            "P",
            "M         9",
            "P",
            "M        -1",
            "P",
            "M        -3",
            "P"
        };

        string[] output = RunGame(20, input);

        Assert.Contains("Mouse evaded Cat", output);
    }

    [Fact]
    public void Cat_Catches_Mouse_Scenario_3()
    {
        string[] input = {
            "27",
            "M         6",
            "M         8",
            "P",
            "M         1",
            "C        22",
            "C         7",
            "P",
            "C       -10",
            "M         1",
            "P",
            "C        11",
            "P",
            "C         5",
            "P",
            "C        -4",
            "P",
            "M         3",
            "P",
            "C        -8",
            "P",
            "M       -11",
            "P",
            "C       -15",
            "P"
        };

        string[] output = RunGame(27, input);

        Assert.Contains("Mouse caught at:  8", output);
    }

    [Fact]
    public void Cat_MovesToMouse_CatchesImmediately()
    {
        string[] output = RunGame(10, "10", "M         5", "C         5");
        Assert.Contains("Mouse caught at:  5", output);
    }
}