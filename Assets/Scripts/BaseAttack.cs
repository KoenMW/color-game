using UnityEngine;

public class BaseAttack : MonoBehaviour, ICharacterMove
{
    [Header("Moves")]
    [SerializeField] private string attackName;

    [TextArea]
    [SerializeField] private string description;

    [SerializeField] private ColorEnum type;
    [SerializeField] private int power;
    [SerializeField] private int accuracy;
    [SerializeField] private int maxPP;

    public string Name => attackName;
    public string Description => description;
    public ColorEnum Type => type;
    public int Power => power;
    public int Accuracy => accuracy;
    public int MaxPP => maxPP;
}