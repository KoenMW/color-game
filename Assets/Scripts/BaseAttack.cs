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
}