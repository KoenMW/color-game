using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private new string name;
    [SerializeField] private Key[] moveKeys = new Key[4];

    private BattleManager battleManager;

    public int playerIndex;

    public BattleCharacter myCharacter;

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

    // needs update to resieve the index of the move to use and the target(s) to use it on
    // needs update to check if the original character is still on the field (not switched out or fainted) before executing the move
    public void ExecuteTurn(int chosenIndex)
    {
        if (!battleManager)
        {
            Debug.LogError("Battlemanager not found");
            return;
        }
        //Check the button that is pressed move the move nu alleen 0
        CharacterMove attack = myCharacter.Moves[chosenIndex]; // 
        attack.Execute(myCharacter, battleManager.GetOthercharacter(playerIndex)); // ! This is currently only for 2 players, will need to be changed for more
    }

    void Update()
    {
        // Keeps trying to find the BattleManager until it succeeds, then registers this player
        if (battleManager == null)
        {
            battleManager = FindAnyObjectByType<BattleManager>();
            playerIndex = battleManager.RegisterPlayer(this);
            Debug.Log($"Player {playerIndex} registered with BattleManager.");
        }
        if (battleManager == null)
        {
            Debug.LogWarning("BattleManager not found!");
            return;
        }
        //kijkt nu naar alle
        for (int i = 0; i < moveKeys.Length; i++)
        {
            if (Keyboard.current[moveKeys[i]].wasPressedThisFrame)
            {
                int chosenIndex = i;

                battleManager.SubmitTurn(new Turn(playerIndex, Speed, () => ExecuteTurn(chosenIndex)));

                break;
            }
        }
    }
}