using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem; // <-- We need this for the modern input system!

public class BattleDialogBox : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;

    public IEnumerator TypeDialog(string line)
    {
        dialogPanel.SetActive(true);
        dialogText.text = "";

        // 1. Typewriter effect: type one letter at a time
        foreach (char letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.02f); // Typing speed
        }

        // 2. Wait for the player to press Space or Left Click (Using the New Input System)
        while (!IsAdvanceKeyPressed())
        {
            yield return null;
        }

        // Wait one extra frame to prevent double-clicking issues
        yield return null;
    }

    // A clean helper method to check the modern inputs safely
    private bool IsAdvanceKeyPressed()
    {
        bool spacePressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool mousePressed = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        return spacePressed || mousePressed;
    }

    public void HideDialog()
    {
        dialogPanel.SetActive(false);
    }
}