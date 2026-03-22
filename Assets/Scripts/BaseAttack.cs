using UnityEngine;

[CreateAssetMenu(fileName = "BaseMove", menuName = "Create/BaseMove")]
public class BaseAttack : CharacterMove
{
    public override void Execute(BattleCharacter user, BattleCharacter target)
    {
        Debug.Log($"{user.name} used {name} on {target.name}!");
        target.TakeDamage(10); // ! This is just a placeholder, will need to be changed for actual moves
    }
}