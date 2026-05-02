using Assets.Scripts.Interfaces;
using System.Collections; // <--- Crucial for Coroutines!
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleUIManager uiManager;

    private BattleState currentState = BattleState.WaitingForPlayers;
    private int turnCounter = 0;

    private Dictionary<int, Player> activePlayers = new Dictionary<int, Player>();
    private List<Turn> queuedTurns = new List<Turn>();

    void Start() { }

    public int RegisterPlayer(Player player)
    {
        activePlayers.Add(activePlayers.Count, player);
        if (activePlayers.Count == 2 && currentState == BattleState.WaitingForPlayers)
        {
            PrepareBattle();
        }
        return activePlayers.Count - 1;
    }

    private void PrepareBattle()
    {
        currentState = BattleState.StartingBattle;
        if (uiManager != null) uiManager.InitializeUI();
        StartFirstRound();
    }

    private void StartFirstRound()
    {
        currentState = BattleState.WaitingForMoves;
        turnCounter = 1;

        foreach (Player p in activePlayers.Values) p.StartNewTurn();
    }

    public void UnregisterPlayer(Player player)
    {
        activePlayers.Remove(player.playerIndex);
    }

    public void SubmitTurn(Turn turn)
    {
        if (currentState != BattleState.WaitingForMoves) return;
        if (queuedTurns.Any(t => t.Player == turn.Player)) return;

        queuedTurns.Add(turn);

        if (queuedTurns.Count == activePlayers.Count)
        {
            currentState = BattleState.Resolving;

            // WE NOW START THE COROUTINE INSTEAD OF INSTANTLY RESOLVING
            StartCoroutine(ResolveRoundRoutine());
        }
    }

    public BattleCharacter GetCharacter(int playerIndex)
    {
        if (activePlayers.ContainsKey(playerIndex)) return activePlayers[playerIndex].activeCharacter;
        return null;
    }

    public BattleCharacter GetOthercharacter(int playerIndex)
    {
        return activePlayers.Where(kvp => kvp.Key != playerIndex).Select(kvp => kvp.Value).ToList()[0].activeCharacter;
    }

    // THIS IS THE NEW COROUTINE ENGINE
    private IEnumerator ResolveRoundRoutine()
    {
        turnCounter++;
        Debug.Log($"--- RESOLVING ROUND {turnCounter} ---");

        queuedTurns = queuedTurns
            .OrderByDescending(t => t.MovePriority)
            .ThenByDescending(t => t.Speed)
            .ToList();

        // 1. Execute each turn slowly, waiting for clicks
        foreach (var turn in queuedTurns)
        {
            if (turn.TurnAction != null)
            {
                // This tells Unity: "Run the player's attack, and DO NOT move on until they click the text box!"
                yield return StartCoroutine(turn.TurnAction.Invoke());
            }

            // Optional: If someone died mid-round, you can stop the next player from attacking here
        }
        queuedTurns.Clear();

        // 2. Hide the text box when the round is fully over
        if (uiManager != null && uiManager.dialogBox != null)
        {
            uiManager.dialogBox.HideDialog();
        }

        // 3. Check for game over or forced switches
        if (currentState == BattleState.GameOver) yield break;

        bool someoneDied = false;
        foreach (Player p in activePlayers.Values)
        {
            if (p.activeCharacter.CurrentHP <= 0) someoneDied = true;
        }

        if (someoneDied)
        {
            currentState = BattleState.WaitingForForcedSwitch;
            Debug.Log("Waiting for fainted characters to be replaced...");
        }
        else
        {
            currentState = BattleState.WaitingForMoves;
            foreach (Player p in activePlayers.Values) p.StartNewTurn();
        }
    }

    public void CheckForcedSwitchesComplete()
    {
        if (currentState != BattleState.WaitingForForcedSwitch) return;

        bool stillWaiting = false;
        foreach (Player p in activePlayers.Values)
        {
            if (p.activeCharacter.CurrentHP <= 0) stillWaiting = true;
        }
        if (!stillWaiting)
        {
            currentState = BattleState.WaitingForMoves;
            foreach (Player p in activePlayers.Values) p.StartNewTurn();
        }
    }

    public void HandlePlayerLoss(Player loser)
    {
        currentState = BattleState.GameOver;
    }
}