using UnityEngine;
using Assets.Scripts.Interfaces;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    private int turnCounter = 0;
    private readonly Dictionary<string, ISubscriber> subscribers = new();


    public void NextTurn(ITurn turn)
    {
        if (turn == null)
        {
            Debug.LogWarning("Turn is null!");
            return;
        }
        if (turn.Player < 0 || turn.Player > 1)
        {
            Debug.LogWarning("Invalid player!");
            return;
        }
        if (turn.Player % 2 != turnCounter % 2)
        {
            Debug.LogWarning("It's not player " + turn.Player + "'s turn!");
            return;
        }

        turnCounter++;
        Debug.Log("Turn: " + turnCounter);
        turn.ExecuteTurn();

        foreach (var subscriber in subscribers.Values)
        {
            subscriber.OnTurnExecuted(turn);
        }
    }

    public void Subscribe(ISubscriber subscriber)
    {
        if (subscriber == null)
        {
            Debug.LogWarning("Subscriber is null!");
            return;
        }

        subscribers.Add(subscriber.Name, subscriber);
    }

    public void Unsubscribe(ISubscriber subscriber)
    {
        if (subscriber == null)
        {
            Debug.LogWarning("Subscriber is null!");
            return;
        }
        subscribers.Remove(subscriber.Name);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
