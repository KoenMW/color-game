using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;


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

    public BattleCharacter GetOthercharacter(int playerIndex)
    {
        return activePlayers.Where(kvp => kvp.Key != playerIndex).Select(kvp => kvp.Value).ToList()[0].activeCharacter; // ! This is currently only for 2 players, will need to be changed for more
    }

    void Update()
    {
    }

    private void ResolveRound()
    {
        turnCounter++;
        Debug.Log($"--- RESOLVING ROUND {turnCounter} ---");

        queuedTurns = queuedTurns
            .OrderByDescending(t => t.MovePriority)
            .ThenByDescending(t => t.Speed)
            .ToList();

        foreach (var turn in queuedTurns)
        {
            turn.ExecuteTurn();
        }
        queuedTurns.Clear();
        if (currentState == BattleState.GameOver)
        {
            Debug.Log("The battle is over! No more turns.");
            return;
        }
        bool someoneDied = false;
        foreach (Player p in activePlayers.Values)
        {
            if (p.activeCharacter.CurrentHP <= 0)
            {
                someoneDied = true;
            }
        }

        if (someoneDied)
        {
            currentState = BattleState.WaitingForForcedSwitch;
            Debug.Log("Waiting for fainted characters to be replaced...");
        }
        else
        {
            currentState = BattleState.WaitingForMoves;
            foreach (Player p in activePlayers.Values)
            {
                p.StartNewTurn();
            }
            Debug.Log("Round over! Waiting for new commands...");
        }
    }
    public void CheckForcedSwitchesComplete()
    {
        if (currentState != BattleState.WaitingForForcedSwitch) return;

        bool stillWaiting = false;
        foreach (Player p in activePlayers.Values)
        {
            if (p.activeCharacter.CurrentHP <= 0)
            {
                stillWaiting = true;
            }
        }
        if (!stillWaiting)
        {
            currentState = BattleState.WaitingForMoves;
            foreach (Player p in activePlayers.Values)
            {
                p.StartNewTurn();
            }
            Debug.Log("All replacements sent out! Starting new round.");
        }
    }
    public void HandlePlayerLoss(Player loser)
    {
        Debug.Log($"!!! PLAYER {loser.playerIndex} HAS LOST THE BATTLE !!!");

        currentState = BattleState.GameOver;

        foreach (Player p in activePlayers.Values)
        {
            if (p != loser)
            {
                Debug.Log($"PLAYER {p.playerIndex} WINS!");
            }
        }
    }

}