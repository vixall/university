using System;
using System.IO;

public class Game
{
    public int size;
    public Player cat;
    public Player mouse;
    public GameState state;

    private GameFiles files;
    private string? finalMessage = null;

    public Game(int size, GameFiles files)
    {
        this.size = size;
        this.files = files;
        cat = new Player("Cat");
        mouse = new Player("Mouse");
        state = GameState.Start;
    }

    public void Run()
    {
        string[] lines = File.ReadAllLines(files.InputPath);
        if (lines.Length == 0) return;

        this.size = int.Parse(lines[0].Trim());

        using (StreamWriter writer = new StreamWriter(files.OutputPath))
        {
            writer.WriteLine("Cat and Mouse");
            writer.WriteLine();
            writer.WriteLine("{0,3} {1,5} {2,9}", "Cat", "Mouse", "Distance");
            writer.WriteLine(new string('-', 19));

            int idx = 1;
            while (state != GameState.End && idx < lines.Length)
            {
                string line = lines[idx].Trim();
                idx++;
                if (string.IsNullOrEmpty(line)) continue;

                if (line[0] == 'P')
                {
                    DoPrintCommand(writer);
                }
                else
                {
                    char cmd = line[0];
                    int value = int.Parse(line.Substring(1).Trim());
                    DoMoveCommand(cmd, value);

                    if (cat.state == State.Playing &&
                        mouse.state == State.Playing &&
                        cat.location == mouse.location)
                    {
                        cat.state = State.Winner;
                        mouse.state = State.Loser;
                        finalMessage = "Mouse caught at:" + string.Format("{0,3}", mouse.location);
                        state = GameState.End;
                    }
                }
            }

            writer.WriteLine(new string('-', 19));
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("Distance traveled:" + "Mouse".PadLeft(8) + "Cat".PadLeft(7));
            writer.WriteLine(mouse.distanceTraveled.ToString().PadLeft(26)
                           + cat.distanceTraveled.ToString().PadLeft(7));
            writer.WriteLine();

            if (finalMessage == null)
                finalMessage = "Mouse evaded Cat";

            writer.WriteLine(finalMessage);
        }
    }

    private void DoMoveCommand(char cmd, int value)
    {
        if (cmd == 'M')
        {
            if (!mouse.IsInitialized)
                mouse.SetInitialPosition(value, size);
            else
                mouse.Move(value, size);
        }
        else if (cmd == 'C')
        {
            if (!cat.IsInitialized)
                cat.SetInitialPosition(value, size);
            else
                cat.Move(value, size);
        }
    }

    private void DoPrintCommand(StreamWriter writer)
    {
        string catStr = (cat.state == State.NotInGame) ? "??" : cat.location.ToString();
        string mouseStr = (mouse.state == State.NotInGame) ? "??" : mouse.location.ToString();

        string distStr;
        if (cat.state == State.NotInGame || mouse.state == State.NotInGame)
            distStr = "";
        else
            distStr = Math.Abs(cat.location - mouse.location).ToString();

        string line = string.Format("{0,3} {1,5} {2,9}", catStr, mouseStr, distStr);
        writer.WriteLine(line.TrimEnd());
    }
}
