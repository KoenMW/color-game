using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Create/Character")]
public class Character : ScriptableObject
{
    [Header("Visuals")]
    [SerializeField] private Sprite portrait;
    // FBX/glTF files are imported as GameObjects (Prefabs), so we store them as such.
    [SerializeField] private GameObject battleModelPrefab;
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip faintAnimation;

    [Header("Info")]
    [SerializeField] private string characterName;
    [SerializeField] private ColorEnum characterColor; // Assuming you have this enum defined elsewhere

    [Header("Base Stats")]
    [SerializeField] private int baseHealth;
    [SerializeField] private int baseAttack;
    [SerializeField] private int baseSpeed;

    [Header("Invested Stats")]
    [SerializeField] private int investedHealth;
    [SerializeField] private int investedAttack;
    [SerializeField] private int investedSpeed;

    [Header("Moves")]
    [SerializeField] private CharacterMove[] moves = new CharacterMove[4]; // Assuming CharacterMove is defined

    // --- PUBLIC PROPERTIES ---
    public Sprite Portrait => portrait;
    public GameObject BattleModelPrefab => battleModelPrefab;
    public AnimationClip IdleAnimation => idleAnimation;
    public AnimationClip FaintAnimation => faintAnimation;

    public string Name => characterName;
    public ColorEnum Color => characterColor;

    public int MaxHealth => baseHealth + investedHealth;
    public int Attack => baseAttack + investedAttack;
    public int Speed => baseSpeed + investedSpeed;

    public CharacterMove[] Moves => moves;

    // --- VALIDATION ---
    private void OnValidate()
    {
        int totalInvested = investedHealth + investedAttack + investedSpeed;

        if (totalInvested != 100)
        {
            Debug.LogWarning("Mag niet. Total invested stats must equal 100.");
        }
    }

    // --- METHODS ---
    // ScriptableObjects do not use Start(), Awake(), or Update(). 
    // Modifying baseSpeed directly here will permanently alter the asset file.
    // Instead, use a method like this to get a randomized speed when your BattleManager asks for it:
    public int GetTemporaryTestSpeed()
    {
        return Speed + UnityEngine.Random.Range(0, 50);
    }
}