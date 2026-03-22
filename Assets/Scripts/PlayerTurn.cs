using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTurn : MonoBehaviour, ITurn
{
    [SerializeField] private int player;
    [SerializeField] private new string name;
    [SerializeField] private Key turnKey;

    private BattleCharacter myCharacter;

    public int Player => player;
    public int Speed => myCharacter != null ? myCharacter.CurrentSpeed : 0;

    public Key TurnKey => turnKey;

    void Start()
    {
        SetupCharacter();
    }

    private void SetupCharacter()
    {
        Character birdData = Resources.Load<Character>("Objects/Bird");
        if (birdData == null) return;

        GameObject characterObject = new GameObject($"Active_Bird_Player{player}");
        myCharacter = characterObject.AddComponent<BattleCharacter>();
        myCharacter.InjectCharacterData(birdData);
    }

    public void ExecuteTurn()
    {
        Debug.Log($"Player {player}'s {myCharacter.gameObject.name} attacks with {Speed} Speed!");
    }
}