using System;
using System.Collections;

public class Turn
{
    public int Player { get; }
    public int Speed { get; }
    public MovePriority MovePriority { get; }

    // This holds the Coroutine from the Player script
    public Func<IEnumerator> TurnAction { get; }

    public Turn(int player, int speed, MovePriority priority, Func<IEnumerator> action)
    {
        Player = player;
        Speed = speed;
        MovePriority = priority;
        TurnAction = action;
    }
}