public class GameFiles
{
    public string InputPath { get; }
    public string OutputPath { get; }

    public GameFiles(string inputPath, string outputPath)
    {
        InputPath = inputPath;
        OutputPath = outputPath;
    }
}
