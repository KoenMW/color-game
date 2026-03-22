using UnityEngine;
using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

// This defines our exact rules for the State Machine
public enum BattleState
{
    WaitingForMoves,
    Resolving
}

public class BattleManager : MonoBehaviour
{
    // The Boss starts by waiting for input
    private BattleState currentState = BattleState.WaitingForMoves;
    private int turnCounter = 0;

    private Dictionary<int, Player> activePlayers = new Dictionary<int, Player>();
    private List<Turn> queuedTurns = new List<Turn>();

    void Start()
    {
    }

    public int RegisterPlayer(Player player)
    {
        activePlayers.Add(activePlayers.Count, player);
        return activePlayers.Count - 1;
    }

    public void UnregisterPlayer(Player player)
    {
        activePlayers.Remove(player.playerIndex);
    }

    // This is called by players to submit their turn for the current round
    public void SubmitTurn(Turn turn)
    {
        if (currentState != BattleState.WaitingForMoves)
        {
            Debug.LogWarning($"Player {turn.Player} tried to submit a turn, but we're currently {currentState}!");
            return;
        }
        if (queuedTurns.Any(t => t.Player == turn.Player))
        {
            Debug.LogWarning($"Player {turn.Player} already submitted a turn this round!");
            return;
        }

        queuedTurns.Add(turn);
        Debug.Log($"Player {turn.Player} submitted a turn with Speed {turn.Speed}.");

        if (queuedTurns.Count == activePlayers.Count)
        {
            currentState = BattleState.Resolving;
            ResolveRound();
        }
    }

    void Update()
    {
    }

    private void ResolveRound()
    {
        turnCounter++;
        Debug.Log($"--- RESOLVING ROUND {turnCounter} ---");

        queuedTurns = queuedTurns.OrderByDescending(t => t.Speed).ToList();

        foreach (var turn in queuedTurns)
        {
            turn.ExecuteTurn();
        }
        queuedTurns.Clear();
        currentState = BattleState.WaitingForMoves;
        Debug.Log("Round over! Boss is waiting for new commands...");
    }
}