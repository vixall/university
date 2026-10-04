using System.IO;

public static class TestRunner
{
    public static string Run(TestCase tc)
    {
        string inputPath = Path.GetTempFileName();
        string outputPath = Path.GetTempFileName();

        try
        {
            File.WriteAllLines(inputPath, tc.InputLines);

            var files = new GameFiles(inputPath, outputPath);
            var game = new Game(tc.BoardSize, files);
            game.Run();

            return File.ReadAllText(outputPath);
        }
        finally
        {
            File.Delete(inputPath);
            File.Delete(outputPath);
        }
    }
}
