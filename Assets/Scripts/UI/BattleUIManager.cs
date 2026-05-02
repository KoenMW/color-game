using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    [Header("Game Logic References")]
    public BattleManager battleManager;

    [Header("UI Element References")]
    public HealthBarUI player1HealthBar;
    public HealthBarUI player2HealthBar;
    public BattleDialogBox dialogBox;

    // This is called by the BattleManager when the battle officially starts
    public void InitializeUI()
    {
        // 1. Get the actual characters from the BattleManager
        // (This uses the GetCharacter method we just added to your BattleManager)
        BattleCharacter p1Character = battleManager.GetCharacter(0);
        BattleCharacter p2Character = battleManager.GetCharacter(1);

        // 2. Tell the individual health bars to run their "Setup" methods
        if (p1Character != null && player1HealthBar != null)
        {
            player1HealthBar.Setup(p1Character);
        }
        else
        {
            Debug.LogWarning("Could not set up Player 1 Health Bar. Is the character or UI missing?");
        }

        if (p2Character != null && player2HealthBar != null)
        {
            player2HealthBar.Setup(p2Character);
        }

        Debug.Log("UI Manager has successfully linked the health bars!");
    }
    // Call this whenever a player sends out a new character
    public void UpdateCharacterUI(int playerIndex, BattleCharacter newCharacter)
    {
        if (playerIndex == 0 && player1HealthBar != null)
        {
            player1HealthBar.Setup(newCharacter);
        }
        else if (playerIndex == 1 && player2HealthBar != null)
        {
            player2HealthBar.Setup(newCharacter);
        }

        Debug.Log($"UI Manager linked Health Bar {playerIndex} to {newCharacter.gameObject.name}!");
    }
}