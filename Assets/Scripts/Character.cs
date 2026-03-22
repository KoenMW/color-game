using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Create/Character")]
public class Character : ScriptableObject
{
    [SerializeField] private Sprite portrait;
    [SerializeField] private Sprite battleSprite;
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip faintAnimation;

    [SerializeField] private string characterName;
    [SerializeField] private ColorEnum characterColor;

    [SerializeField] private int baseHealth;
    [SerializeField] private int baseAttack;
    [SerializeField] private int baseSpeed;

    [SerializeField] private int investedHealth;
    [SerializeField] private int investedAttack;
    [SerializeField] private int investedSpeed;

    [SerializeField] private CharacterMove[] moves = new CharacterMove[4];

    public Sprite Portrait => portrait;
    public Sprite BattleSprite => battleSprite;
    public AnimationClip IdleAnimation => idleAnimation;
    public AnimationClip FaintAnimation => faintAnimation;

    public int MaxHealth => baseHealth + investedHealth;
    public int Attack => baseAttack + investedAttack;
    public int Speed => baseSpeed + investedSpeed;

    public string Name => characterName;
    public ColorEnum Color => characterColor;

    public CharacterMove[] Moves => moves;

    private void OnValidate()
    {
        int totalInvested = investedHealth + investedAttack + investedSpeed;

        if (totalInvested != 100)
        {
            Debug.LogWarning("Mag niet");
        }
    }
}