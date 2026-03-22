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

    private List<PlayerTurn> activePlayers = new List<PlayerTurn>();
    private List<ITurn> queuedTurns = new List<ITurn>();

    void Start()
    {
        // When the game starts, the Boss finds all players automatically
        activePlayers = FindObjectsByType<PlayerTurn>(FindObjectsSortMode.None).ToList();
        Debug.Log($"Battle Boss found {activePlayers.Count} players. Waiting for moves...");
    }

    void Update()
    {
        // The Boss ONLY listens to the keyboard if it is the "Waiting" phase
        if (currentState == BattleState.WaitingForMoves)
        {
            CheckPlayerInputs();
        }
    }

    private void CheckPlayerInputs()
    {
        foreach (var player in activePlayers)
        {
            // If this player already locked in a move, skip them
            if (queuedTurns.Any(t => t.Player == player.Player)) continue;

            // Check if THIS specific player's key was pressed
            if (Keyboard.current != null && Keyboard.current[player.TurnKey].wasPressedThisFrame)
            {
                queuedTurns.Add(player);
                Debug.Log($"Player {player.Player} locked in a move with Speed {player.Speed}.");

                // If everyone has locked in, change the state!
                if (queuedTurns.Count == activePlayers.Count)
                {
                    currentState = BattleState.Resolving; // Lock out the keyboard!
                    ResolveRound();
                }
            }
        }
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