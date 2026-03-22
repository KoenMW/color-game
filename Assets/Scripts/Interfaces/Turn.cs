using UnityEngine;

public class Turn
{
    public int Player { get; }

    public int Speed { get; }

    public Turn(int player, int speed)
    {
        Player = player;
        Speed = speed;
    }
    public void ExecuteTurn()
    {
        Debug.Log($"Player {Player} executes their turn!");
    }
}