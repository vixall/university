using System;

class Program
{
    static void Main(string[] args)
    {
        RunTest("1.ChaseData.txt", "1.PursuitLog.txt", 16);
        RunTest("2.ChaseData.txt", "2.PursuitLog.txt", 20);
        RunTest("3.ChaseData.txt", "3.PursuitLog.txt", 27);

        Console.WriteLine("Готово! Файлы PursuitLog.txt созданы.");
    }

    static void RunTest(string input, string output, int size)
    {
        GameFiles files = new GameFiles(input, output);
        Game game = new Game(size, files);
        game.Run();
    }
}
