using UnityEngine;
using Assets.Scripts.Interfaces;
using System;
using UnityEngine.InputSystem;

public class PlayerTurn : MonoBehaviour, ITurn, ISubscriber
{
    [SerializeField] private int player;
    [SerializeField] private new string name;

    [SerializeField] private Key turnKey;

    private BattleManager battleManager;

    public int Player => player;

    string ISubscriber.Name => name;

    public void ExecuteTurn()
    {
        Debug.Log("Player " + player + " executed their turn.");
    }

    public void OnTurnExecuted(ITurn turn)
    {
        Debug.Log("Player " + player + " had their turn executed.");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battleManager = FindFirstObjectByType<BattleManager>();
        if (battleManager != null)
        {
            battleManager.Subscribe(this);
        }
        else
        {
            Debug.LogWarning("BattleManager not found!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (battleManager == null)
        {
            Debug.LogWarning("BattleManager not found!");
            return;
        }
        if (Keyboard.current != null && Keyboard.current[turnKey].wasPressedThisFrame)
        {
            battleManager.NextTurn(this);
        }
    }
}
