using Unity.VisualScripting;
using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    [SerializeField] private Character characterData;

    [SerializeField] private int currentHP;
    [SerializeField] private int currentAttack;
    [SerializeField] private int currentSpeed;

    [SerializeField] private Sprite portrait;
    [SerializeField] private Sprite battleSprite;
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip faintAnimation;
    [SerializeField] private CharacterMove[] moves;

    public Character Data => characterData;
    public ColorEnum Color =>characterData.Color;
    public int CurrentHP => currentHP;
    public int CurrentAttack => currentAttack;
    public int CurrentSpeed => currentSpeed;
    public CharacterMove[] Moves => moves;
    public Sprite Portrait => portrait;
    public Sprite BattleSprite => battleSprite;
    public AnimationClip IdleAnimation => idleAnimation;
    public AnimationClip FaintAnimation => faintAnimation;

    public void InjectCharacterData(Character data)
    {
        characterData = data;
        InitializeCharacter();
    }
    void Start()
    {
        if (characterData != null)
        {
            InitializeCharacter();
            Debug.LogWarning($"Now Character Data assigned to {gameObject.name}!");
        }
        else
        {
            Debug.LogWarning($"No Character Data assigned to {gameObject.name}!");
        }
    }

    private void InitializeCharacter()
    {
        currentHP = characterData.MaxHealth;
        currentAttack = characterData.Attack;
        currentSpeed = characterData.Speed;

        portrait = characterData.Portrait;
        battleSprite = characterData.BattleSprite;
        idleAnimation = characterData.IdleAnimation;
        faintAnimation = characterData.FaintAnimation;
        if (characterData.Moves != null)
        {
            moves = new CharacterMove[characterData.Moves.Length];
            for (int i = 0; i < characterData.Moves.Length; i++)
            {
                moves[i] = characterData.Moves[i];
            }
        }
    }
    public void ChangeMove1(CharacterMove move)
    {
        moves[0] = move;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        Debug.Log($"{gameObject.name} took {damageAmount} damage! Remaining HP: {currentHP} / {characterData.MaxHealth}");
        if (currentHP < 0)
        {
            currentHP = 0;
        }

        if (currentHP == 0)
        {
            HandleFaint();
        }
    }

    public void ChangeAttack(int amount)
    {
        currentAttack += amount;
    }

    public void ChangeSpeed(int amount)
    {
        currentSpeed += amount;
    }

    private void HandleFaint()
    {
        Debug.Log("faint");
    }
}