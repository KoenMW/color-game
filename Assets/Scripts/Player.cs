using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private new string name;
    [SerializeField] private Key turnKey;

    private BattleManager battleManager;

    public int playerIndex;

    private BattleCharacter myCharacter;

    public int Speed => myCharacter != null ? myCharacter.CurrentSpeed : 0;

    void Start()
    {
        SetupCharacter();
    }

    private void SetupCharacter()
    {
        Character birdData = Resources.Load<Character>("Objects/Bird");
        if (birdData == null) return;

        GameObject characterObject = new GameObject($"Active_Bird_Player{playerIndex}");
        myCharacter = characterObject.AddComponent<BattleCharacter>();
        myCharacter.InjectCharacterData(birdData);
    }

    public void ExecuteTurn()
    {
        Debug.Log($"Player {playerIndex}'s {myCharacter.gameObject.name} attacks with {Speed} Speed!");
    }

    void Update()
    {
        // Keeps trying to find the BattleManager until it succeeds, then registers this player
        if (battleManager == null)
        {
            battleManager = FindObjectOfType<BattleManager>();
            playerIndex = battleManager.RegisterPlayer(this);
            Debug.Log($"Player {playerIndex} registered with BattleManager.");
        }
        if (battleManager == null)
        {
            Debug.LogWarning("BattleManager not found!");
            return;
        }

        // Check for turn key press to submit turn
        // ! This is only for testing of the move system
        if (Keyboard.current != null && Keyboard.current[turnKey].wasPressedThisFrame)
        {
            battleManager.SubmitTurn(new Turn(playerIndex, Speed));
        }
    }
}