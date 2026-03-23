using UnityEngine;

[CreateAssetMenu(fileName = "BaseMove", menuName = "Create/BaseMove")]
public class BaseAttack : CharacterMove

{
    public override void Execute(BattleCharacter user, BattleCharacter target)
    {
        float damageMultiplier = user.CurrentAttack / 100f;

        int finalDamage = Mathf.RoundToInt(damageMultiplier * Power * GetDamageAmplifier(this.Type, target.Color));

        Debug.Log($"{user.gameObject.name} used {Name} on {target.gameObject.name} for {finalDamage} {GetDamageAmplifier(this.Type, target.Color)} damage!");

        target.TakeDamage(finalDamage);
    }
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