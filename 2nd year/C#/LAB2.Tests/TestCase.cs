public class TestCase
{
    public string Name { get; }
    public int BoardSize { get; }
    public string[] InputLines { get; }
    public string[] ExpectedMarkers { get; }

    public TestCase(string name, int boardSize, string[] inputLines, string[] expectedMarkers)
    {
        Name = name;
        BoardSize = boardSize;
        InputLines = inputLines;
        ExpectedMarkers = expectedMarkers;
    }
}
