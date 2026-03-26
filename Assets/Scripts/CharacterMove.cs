using UnityEngine;

public abstract class CharacterMove : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string moveName;
    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private ColorEnum type;

    [Header("Combat Stats")]
    [SerializeField] private int power;
    [SerializeField] private int accuracy;
    [SerializeField] private int maxPP;
    public string Name => moveName;
    public string Description => description;
    public ColorEnum Type => type;
    public int Power => power;
    public int Accuracy => accuracy;
    public int MaxPP => maxPP;

    public abstract void Execute(BattleCharacter user, BattleCharacter target);
    public static float GetDamageAmplifier(ColorEnum attacker, ColorEnum defender)
    {
        if (attacker == ColorEnum.White || attacker == ColorEnum.Black ||
            defender == ColorEnum.White || defender == ColorEnum.Black)
        {
            return 1.0f;
        }

        int attIndex = (int)attacker;
        int defIndex = (int)defender;

        int clockwiseDistance = (defIndex - attIndex + 12) % 12;

        return clockwiseDistance switch
        {
            0 => 1.0f,
            1 => 1.2f,
            2 => 1.5f,
            3 => 1.75f,
            4 => 2.0f,
            5 => 1.5f,
            6 => 1.0f,
            7 => 0.8f,
            8 => 0.5f,
            9 => 0.65f,
            10 => 0.75f,
            11 => 0.9f,

            _ => 1.0f
        };
    }
}