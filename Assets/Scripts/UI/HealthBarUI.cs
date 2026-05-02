using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI hpText; 
    private BattleCharacter trackedCharacter;

    // This is the "Setup" method the UI Manager calls!
    public void Setup(BattleCharacter character)
    {
        // Stop tracking the old character if we had one
        if (trackedCharacter != null)
        {
            trackedCharacter.OnHealthChanged -= UpdateVisuals;
        }

        trackedCharacter = character;

        // Start tracking the new character's megaphone
        trackedCharacter.OnHealthChanged += UpdateVisuals;

        // Force it to update immediately so it shows full health at the start
        UpdateVisuals(trackedCharacter.CurrentHP, trackedCharacter.Data.MaxHealth);
    }

    // This runs automatically whenever the character shouts that its health changed
    private void UpdateVisuals(int currentHP, int maxHP)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHP;
            healthSlider.value = currentHP;
        }

        if (hpText != null)
        {
            hpText.text = $"{currentHP} / {maxHP}";
        }
    }

    private void OnDestroy()
    {
        // Clean up our listener if the UI is destroyed to prevent memory leaks
        if (trackedCharacter != null)
        {
            trackedCharacter.OnHealthChanged -= UpdateVisuals;
        }
    }
}