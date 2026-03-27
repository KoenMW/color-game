using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour

{
    [SerializeField] private new string name;

    [SerializeField] private PlayerInputState currentInputState = PlayerInputState.ChoosingAction;
    private int pendingMoveIndex = -1; 

    [SerializeField] private Key[] moveKeys = new Key[4];
    [SerializeField] private Key[] switchKeys = new Key[3]; 
    [SerializeField] private Key cancelKey = Key.Escape;
    [SerializeField] private Character[] startingTeamData = new Character[3];

    private BattleManager battleManager;

    public int playerIndex;

    public BattleCharacter[] team;
    public BattleCharacter activeCharacter;

    public int Speed => activeCharacter != null ? activeCharacter.CurrentSpeed : 0;

    void Start()
    {
        SetupTeam();
    }
    void Update()
    {
        if (Keyboard.current == null) return;
        CheckBattleManager();
        if (currentInputState == PlayerInputState.ChoosingAction)
        {
            DoMove();
        }
        else if (currentInputState == PlayerInputState.ChoosingSwitchTarget)
        {
            SwitchCharacter();
        }
        else if (currentInputState == PlayerInputState.ForcedSwitchTarget)
        {
            HandleForcedSwitch();
        }
    }
    private void SetupTeam()
    {
        team = new BattleCharacter[startingTeamData.Length];

        for (int i = 0; i < startingTeamData.Length; i++)
        {
            if (startingTeamData[i] == null) continue;

            GameObject characterObject = new GameObject($"Player{playerIndex}_{startingTeamData[i].Name}");
            BattleCharacter newChar = characterObject.AddComponent<BattleCharacter>();

  
            newChar.InjectCharacterData(startingTeamData[i]);
            newChar.playerOwner = this;

            team[i] = newChar;

            if (i != 0)
            {
                characterObject.SetActive(false);
            }
        }
        if (team.Length > 0)
        {
            activeCharacter = team[0];
            Debug.Log($"Player {playerIndex} sent out {activeCharacter.gameObject.name}!");
        }
    }
    private void ExecuteTurn(int chosenIndex)
    {
        if (!battleManager)
        {
            Debug.LogError("Battlemanager not found");
            return;
        }
        if (activeCharacter.CurrentHP <= 0)
        {
            Debug.Log($"{activeCharacter.gameObject.name} fainted and cannot attack!");
            return;
        }
        BattleCharacter target = battleManager.GetOthercharacter(playerIndex);

        CharacterMove chosenMove = activeCharacter.Moves[chosenIndex];

        if (chosenMove != null)
        {
            chosenMove.Execute(activeCharacter, target);
        }
        else
        {
            Debug.LogWarning($"Move slot {chosenIndex} is empty!");
        }
    }
    private void DoMove()
    {
        for (int i = 0; i < moveKeys.Length; i++)
        {
            if (Keyboard.current[moveKeys[i]].wasPressedThisFrame)
            {
                int chosenIndex = i;
                CharacterMove chosenMove = activeCharacter.Moves[chosenIndex];

                if (chosenMove == null)
                {
                    Debug.LogWarning($"Player {playerIndex} move slot {chosenIndex} is empty!");
                    continue;
                }

                if (chosenMove is SwitchMove)
                {
                    pendingMoveIndex = chosenIndex;

                    currentInputState = PlayerInputState.ChoosingSwitchTarget;
                    Debug.Log($"[Player {playerIndex}] Selected Switch! Press your Switch Keys to pick a teammate, or Cancel to go back.");
                    return;
                }
                currentInputState = PlayerInputState.Waiting;
                battleManager.SubmitTurn(new Turn(playerIndex, Speed, () => ExecuteTurn(chosenIndex)));
                return;
            }
        }
    }
    private void SwitchCharacter()
    {
        // wil je switchen
        if (Keyboard.current[cancelKey].wasPressedThisFrame)
        {
            currentInputState = PlayerInputState.ChoosingAction;
            Debug.Log($"[Player {playerIndex}] Cancelled switch menu. Choose a move!");
            return;
        }

        for (int i = 0; i < switchKeys.Length; i++)
        {
            if (Keyboard.current[switchKeys[i]].wasPressedThisFrame)
            {
                int switchTargetIndex = i; 

                if (team[switchTargetIndex].CurrentHP <= 0 || team[switchTargetIndex] == activeCharacter)
                {
                    Debug.LogWarning("You can't switch to that character! Pick another or Cancel.");
                    continue;
                }
                activeCharacter.queuedSwitchIndex = switchTargetIndex;
                currentInputState = PlayerInputState.Waiting;
                battleManager.SubmitTurn(new Turn(playerIndex, Speed, () => ExecuteTurn(pendingMoveIndex)));
                return;
            }
        }
    }
    private void CheckBattleManager()
    {
        if (battleManager == null)
        {
            battleManager = FindAnyObjectByType<BattleManager>();
            if (battleManager != null)
            {
                playerIndex = battleManager.RegisterPlayer(this);
                Debug.Log($"Player {playerIndex} registered with BattleManager.");
            }
            return;
        }
    }
    public void PerformSwitch(int newTeamIndex)
    {
        //de switch
        activeCharacter.gameObject.SetActive(false);
        activeCharacter = team[newTeamIndex];
        activeCharacter.gameObject.SetActive(true);

        Debug.Log($"Player {playerIndex} sent out {activeCharacter.gameObject.name}!");
    }
    private void HandleForcedSwitch()
    {
        for (int i = 0; i < switchKeys.Length; i++)
        {
            if (Keyboard.current[switchKeys[i]].wasPressedThisFrame)
            {
                if (team[i].CurrentHP <= 0 || team[i] == activeCharacter)
                {
                    Debug.LogWarning("That character is dead! Pick another.");
                    continue;
                }
                currentInputState = PlayerInputState.Waiting;
                PerformSwitch(i);

                battleManager.CheckForcedSwitchesComplete();

                return;
            }
        }
    }
    public void StartNewTurn()
    {
        if (currentInputState == PlayerInputState.ForcedSwitchTarget)
        {
            return;
        }
        currentInputState = PlayerInputState.ChoosingAction;
    }
    public void TriggerForcedSwitch()
    {
        bool hasAliveTeammates = false;
        foreach (BattleCharacter teammate in team)
        {
            if (teammate.CurrentHP > 0)
            {
                hasAliveTeammates = true;
                break; 
            }
        }

        if (!hasAliveTeammates)
        {
            Debug.Log($"[Player {playerIndex}] has no living characters left!");
            battleManager.HandlePlayerLoss(this); 
            return; 
        }

        currentInputState = PlayerInputState.ForcedSwitchTarget;
        Debug.Log($"[Player {playerIndex}] {activeCharacter.gameObject.name} fainted! You MUST choose a replacement.");
    }
}
