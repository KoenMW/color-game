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
}