using UnityEngine;
using System;

public class Turn
{
    public int Player { get; }

    public int Speed { get; }

    private Action ExecuteAction;

    public Turn(int player, int speed, Action executeAction)
    {
        Player = player;
        Speed = speed;
        ExecuteAction = executeAction;
    }

    public void ExecuteTurn()
    {
        ExecuteAction.Invoke();
    }
}