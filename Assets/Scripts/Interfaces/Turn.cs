using UnityEngine;
using System;

public class Turn
{
    public int Player { get; }

    public int Speed { get; }
    public MovePriority MovePriority { get; }

    private Action ExecuteAction;

    public Turn(int player, int speed, MovePriority priority, Action executeAction)
    {
        Player = player;
        Speed = speed;
        MovePriority = priority;
        ExecuteAction = executeAction;
    }

    public void ExecuteTurn()
    {
        ExecuteAction.Invoke();
    }
}