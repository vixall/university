using System;

public class Player
{
    public string name;
    public int location;
    public State state = State.NotInGame;
    public int distanceTraveled = 0;

    private bool positionInitialized = false;

    public Player(string name)
    {
        this.name = name;
        this.location = -1;
    }

    public bool IsInitialized
    {
        get { return positionInitialized; }
    }

    public void SetInitialPosition(int pos, int boardSize)
    {
        location = Normalize(pos, boardSize);
        state = State.Playing;
        positionInitialized = true;
    }

    public void Move(int steps, int boardSize)
    {
        if (state != State.Playing) return;
        location = Normalize(location + steps, boardSize);
        distanceTraveled += Math.Abs(steps);
    }

    public static int Normalize(int pos, int boardSize)
    {
        return ((pos - 1) % boardSize + boardSize) % boardSize + 1;
    }
}
