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

    public Character Data => characterData;
    public int CurrentHP => currentHP;
    public int CurrentAttack => currentAttack;
    public int CurrentSpeed => currentSpeed;

    public Sprite Portrait => portrait;
    public Sprite BattleSprite => battleSprite;
    public AnimationClip IdleAnimation => idleAnimation;
    public AnimationClip FaintAnimation => faintAnimation;


    void Start()
    {
        if (characterData != null)
        {
            InitializeCharacter();
        }
        else
        {
            Debug.LogError($"No Character Data assigned to {gameObject.name}!");
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
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;

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